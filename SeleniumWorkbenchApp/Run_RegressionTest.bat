GOTO SKIP_BLOCK
for vstest.console.exe (Total 36 testcases):
CD %curdir%\microsoft.testplatform.17.14.1\tools\net462\Common7\IDE\Extensions\TestPlatform
vstest.console.exe "%curdir%\SeleniumWorkbenchApp\bin\x64\Debug\net8.0-windows\WorkbenchApp.UITest.dll" /Tests:^
TC001_login_with_valid_accountAsync,^
TC001_TotalEndowment_NavigateToAnotherPage,^
TC002_TotalEndowment_sort_Asset,^
TC001_PublicFund_default_data,^
TC002_PublicFund_sort_Asset,^
TC003_PublicFund_groupByAssetClass_and_ShowFundsOnly,^
TC004_PublicFund_Rating,^
TC005_PublicFund_IconLinkToFundDetails,^
TC001_PrivateFund_default_data,^
TC002_PrivateFund_sort_Asset,^
TC003_PrivateFund_GroupByAssetClass_and_GroupByManager,^
TC004_PrivateFund_Rating,^
TC001_SearchFund_Albourne_FundInfoAsync,^
TC002_SearchFund_AlbourneManual_FundInfo,^
TC003_SearchFund_Solovis_FundInfoAsync,^
TC004_SearchFund_AEvestment_FundInfoAsync,^
TC005_SearchFund_TEvestment_FundInfoAsync,^
TC006_SearchFund_Cambridge_FundInfoAsync,^
TC007_SearchFund_Pipeline_PublicFundInfo,^
TC008_SearchFund_Pipeline_PrivateFundInfo,^
TC001_SinglePublicReport_Albourne_NoInput,^
TC002_SinglePublicReport_AlbourneManual_Input,^
TC003_SinglePublicReport_Solovis_NoInput,^
TC004_SinglePublicReport_AEvestment_NoInput,^
TC005_SinglePublicReport_TEvestment_NoInput,^
TC001_SingleManagerDashboard_SourceManual,^
TC002_SingleManagerDashboard_SourceCambridge,^
TC001_DxDReport,^
TC001_Add_PublicFund_AddNewFund,^
TC002_Add_PrivateFund_AddNewFund,^
TC003_Add_PipelineFund_EditFund,^
TC004_Add_PipelineFund_AddNewFund,^
TC001_UploadFile_PublicFund,^
TC002_UploadFile_PrivateFund,^
TC001_ScenarioTest,^
TC001_Pipeline_Page

for dotnet test:
--filter "Name=TC001_login_with_valid_accountAsync|Name=TC002_TotalEndowment_sort_Asset"

+ run multi testcase: (example below)
set "TEST_LIST="
set TEST_LIST=!TEST_LIST!^|Name=TC001_login_with_valid_accountAsync
set TEST_LIST=!TEST_LIST!^|Name=TC002_TotalEndowment_sort_Asset
...
:SKIP_BLOCK
@ECHO OFF
SETLOCAL EnableDelayedExpansion

set curdir=%cd%

REM --- Liệt kê các test case của bạn tại đây ---
set "TEST_LIST="
set TEST_LIST=!TEST_LIST!^|Name=TC001_login_with_valid_accountAsync
set TEST_LIST=!TEST_LIST!^|Name=TC001_SearchFund_Albourne_FundInfoAsync
set TEST_LIST=!TEST_LIST!^|Name=TC003_SearchFund_Solovis_FundInfoAsync
set TEST_LIST=!TEST_LIST!^|Name=TC004_SearchFund_AEvestment_FundInfoAsync
set TEST_LIST=!TEST_LIST!^|Name=TC005_SearchFund_TEvestment_FundInfoAsync
set TEST_LIST=!TEST_LIST!^|Name=TC006_SearchFund_Cambridge_FundInfoAsync

REM --- Tự động tạo chuỗi filter ---
set "FILTER_STRING=!TEST_LIST:~1!"

ECHO Changing directory and executing tests...
CD %curdir%\microsoft.testplatform.17.14.1\tools\net462\Common7\IDE\Extensions\TestPlatform

dotnet test "%curdir%\SeleniumWorkbenchApp\WorkbenchApp.UITest.csproj" ^
 --filter "!FILTER_STRING!" ^
 -p:Platform=x64 ^
 --logger "console;verbosity=detailed"
ECHO.
timeout 3 >nul

REM === Gửi mail report mới nhất ===
python "%curdir%/send_latest_report_Graph_Delegated.py"

EXIT
ENDLOCAL