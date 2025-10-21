#!/usr/bin/env python3
"""
send_latest_report_Graph_Delegated.py
-------------------------------------
Gửi email kèm báo cáo HTML (mới nhất trong TestResults/)
qua Microsoft Graph API (Delegated OAuth2) — KHÔNG CẦN ADMIN.

Lần đầu tiên chạy:
- Script sẽ mở trình duyệt yêu cầu bạn đăng nhập Microsoft.
- Sau đó lưu token vào token_cache.json.
- Những lần sau tự động refresh token, không cần đăng nhập lại.

Yêu cầu:
    pip install msal requests python-dotenv
"""

import os
import re
import glob
import json
import base64
import requests
from msal import PublicClientApplication
from typing import List, Optional

# ------------- CONFIG ----------------
BASE_DIR = os.path.dirname(os.path.abspath(__file__))
REPORT_DIR = os.path.join(BASE_DIR, "TestResults")

# 🔧 Tốt nhất nên lưu trong file .env hoặc biến môi trường
TENANT_ID = os.environ.get("AZ_TENANT_ID", "your azure tenant_id")
CLIENT_ID = os.environ.get("AZ_CLIENT_ID", "your azure client_id")
SENDER = os.environ.get("SENDER_EMAIL", "yourmail@domaincomp.com")
RECIPIENTS_ENV = os.environ.get("RECIPIENTS", "")
RECIPIENTS = [r.strip() for r in RECIPIENTS_ENV.split(",")] if RECIPIENTS_ENV else ["recipient1@domaincomp.com", "recipient2@domaincomp.com"]

AUTHORITY = f"https://login.microsoftonline.com/{TENANT_ID}"
SCOPES = ["Mail.Send", "User.Read"]

TOKEN_CACHE_FILE = os.path.join(BASE_DIR, "token_cache.json")
GRAPH_API = "https://graph.microsoft.com/v1.0"

# -------------------------------------

def load_token_cache():
    if os.path.exists(TOKEN_CACHE_FILE):
        with open(TOKEN_CACHE_FILE, "r", encoding="utf-8") as f:
            return json.load(f)
    return {}

def save_token_cache(cache):
    with open(TOKEN_CACHE_FILE, "w", encoding="utf-8") as f:
        json.dump(cache, f)

def acquire_token() -> str:
    """
    Lấy access token (refresh tự động nếu cache còn hợp lệ).
    """
    app = PublicClientApplication(CLIENT_ID, authority=AUTHORITY)

    cache = load_token_cache()
    if "refresh_token" in cache:
        result = app.acquire_token_by_refresh_token(cache["refresh_token"], scopes=SCOPES)
        if "access_token" in result:
            cache["refresh_token"] = result.get("refresh_token", cache["refresh_token"])
            save_token_cache(cache)
            return result["access_token"]

    # Nếu chưa có token, mở trình duyệt login
    flow = app.initiate_device_flow(scopes=SCOPES)
    if "user_code" not in flow:
        raise Exception("Device flow initiation failed")

    print("\n🔑 Please sign in to Microsoft account:")
    print(flow["message"])  # hiển thị URL + code
    result = app.acquire_token_by_device_flow(flow)

    if "access_token" in result:
        cache["refresh_token"] = result.get("refresh_token")
        save_token_cache(cache)
        return result["access_token"]

    raise Exception(f"Authentication failed: {result.get('error_description')}")

# ----------- Report Helpers -----------
def get_latest_report() -> Optional[str]:
    files = glob.glob(os.path.join(REPORT_DIR, "*.html"))
    return max(files, key=os.path.getmtime) if files else None

def get_environment(html_file: str) -> str:
    try:
        with open(html_file, "r", encoding="utf-8") as f:
            content = f.read()
        match = re.search(r"<td>Environment</td>\s*<td>(.*?)</td>", content)
        return match.group(1).strip() if match else "Unknown"
    except:
        return "Unknown"

def get_test_result(html_file: str) -> str:
    try:
        with open(html_file, "r", encoding="utf-8") as f:
            content = f.read()
        failed = re.search(r"Tests Failed</p>\s*<h3>(\d+)</h3>", content)
        if failed:
            return "PASS" if int(failed.group(1)) == 0 else "FAIL"
        return "UNKNOWN"
    except:
        return "ERROR"

# ----------- Mail Builders -----------
def make_attachment(file_path: str) -> dict:
    with open(file_path, "rb") as f:
        b64 = base64.b64encode(f.read()).decode("utf-8")
    return {
        "@odata.type": "#microsoft.graph.fileAttachment",
        "name": os.path.basename(file_path),
        "contentType": "text/html",
        "contentBytes": b64
    }

def build_message(subject: str, body: str, recipients: List[str], attachment_path: Optional[str]) -> dict:
    message = {
        "subject": subject,
        "body": {"contentType": "Text", "content": body},
        "toRecipients": [{"emailAddress": {"address": r}} for r in recipients]
    }
    if attachment_path:
        message["attachments"] = [make_attachment(attachment_path)]
    return {"message": message, "saveToSentItems": True}

# ----------- Main send function -----------
def send_latest_report():
    latest = get_latest_report()
    if not latest:
        print("⚠️ No HTML report found in TestResults/")
        return

    env = get_environment(latest)
    result = get_test_result(latest)

    if result == "PASS":
        subject = f"✅ Aloha [{env.upper()}] TEST PASSED - Automation Report"
        body = f"Hi,\n\n✅ All tests passed successfully in {env.upper()} environment!\n\nAttached is the latest test report."
    elif result == "FAIL":
        subject = f"❌ [{env.upper()}] TEST FAILED - Automation Report"
        body = f"Hi,\n\n❌ Some tests failed in {env.upper()} environment!\n\nPlease check the attached report for details."
    else:
        subject = f"⚠️ [{env.upper()}] TEST REPORT - Automation Results"
        body = f"Hi,\n\n⚠️ Test result unknown in {env.upper()} environment.\n\nAttached is the latest test report."

    print(f"📦 Preparing to send report: {os.path.basename(latest)}")

    try:
        token = acquire_token()
    except Exception as e:
        print(f"❌ Failed to acquire token: {e}")
        return

    headers = {"Authorization": f"Bearer {token}", "Content-Type": "application/json"}
    payload = build_message(subject, body, RECIPIENTS, latest)

    print(f"📨 Sending mail via Graph API...")
    res = requests.post(f"{GRAPH_API}/me/sendMail", headers=headers, json=payload)
    if res.status_code in (202, 200):
        print("✅ Email sent successfully!")
    else:
        print(f"❌ Send failed ({res.status_code}): {res.text}")

# -------- Entry point --------
if __name__ == "__main__":
    send_latest_report()
