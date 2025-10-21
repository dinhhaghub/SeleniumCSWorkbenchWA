import os, glob
import yagmail
import re

# 📌 Cấu hình
# 📌 lấy thư mục nơi chứa file send_latest_report.py
BASE_DIR = os.path.dirname(os.path.abspath(__file__))
REPORT_DIR = os.path.join(BASE_DIR, "TestResults")

SENDER = "yourgmail"
PASSWORD = "yourpass"
RECIPIENTS = ["reciepient1@gmail.com", "reciepient2@gmail.com"]

def get_latest_report():
    files = glob.glob(os.path.join(REPORT_DIR, "*.html"))
    if not files:
        return None
    return max(files, key=os.path.getmtime)

def get_environment(html_file):
    """Lấy thông tin Environment từ file HTML"""
    try:
        with open(html_file, 'r', encoding='utf-8') as file:
            content = file.read()
        
        # Tìm Environment trong bảng System/Environment
        env_match = re.search(r'<td>Environment</td>\s*<td>(.*?)</td>', content)
        return env_match.group(1).strip() if env_match else "Unknown"
    except:
        return "Unknown"

def get_test_result(html_file):
    """Lấy kết quả test từ file HTML"""
    try:
        with open(html_file, 'r', encoding='utf-8') as file:
            content = file.read()
        
        failed_match = re.search(r'Tests Failed</p>\s*<h3>(\d+)</h3>', content)
        if failed_match:
            failed_count = int(failed_match.group(1))
            return "PASS" if failed_count == 0 else "FAIL"
        return "UNKNOWN"
    except:
        return "ERROR"

def send_report():
    latest_report = get_latest_report()
    
    if not latest_report:
        print("No report file found!")
        return
    
    environment = get_environment(latest_report)
    result = get_test_result(latest_report)
    
    # Tạo subject với Environment
    if result == "PASS":
        subject = f"✅ Aloha [{environment.upper()}] TEST PASSED - Automation Report"
        body = f"Hi,\n\n✅ All tests passed successfully in {environment.upper()} environment!\n\nAttached is the latest test report."
    elif result == "FAIL":
        subject = f"❌ [{environment.upper()}] TEST FAILED - Automation Report"
        body = f"Hi,\n\n❌ Some tests failed in {environment.upper()} environment!\n\nPlease check the attached report for details."
    else:
        subject = f"⚠️ [{environment.upper()}] TEST REPORT - Automation Results"
        body = f"Hi,\n\n⚠️ Test result unknown in {environment.upper()} environment.\n\nAttached is the latest test report."
    
    try:
        yag = yagmail.SMTP(SENDER, PASSWORD)
        yag.send(
            to=RECIPIENTS,
            subject=subject,
            contents=[body, latest_report]
        )
        print(f"Sent: {latest_report} | Environment: {environment} | Result: {result}")
    except Exception as e:
        print(f"Error: {str(e)}")

if __name__ == "__main__":
    send_report()