using AventStack.ExtentReports;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SeleniumGendKS.Core.DBConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using WorkbenchApp.UITest.Pages;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(10)]
    internal class UploadFileTest : BaseTestCase
    {
        // Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [Test, Category("Regression Testing")]
        public void TC001_UploadFile_PublicFund()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string? inputSearch = null;
            string? managerName = null;
            string? fundName = null;
            int? fundid = null;
            const string sourceIcon = "M";
            //string message = "This fund does not have Return data. Select another data source or upload the fund data.";
            DateTime timestampOrg = DateTime.UtcNow.AddDays(-1);
            string timestamp = timestampOrg.ToString("yyyy-MM-dd 00:00:00.000"); //DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string videoFileName = "UploadFileTestTC001";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Upload File Test - TC001");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                string? exposureNRowCountVal = null, exposureStartDateVal = null, exposureEndDateVal = null;
                if (urlInstance.Contains("lab"))
                {
                    inputSearch = "qa test 04";
                    managerName = "QA Test 04";
                    fundName = "Main Fund of QA Test 04 update 01";
                    fundid = 19;
                    //exposureNRowCountVal = "2";
                    //exposureStartDateVal = "03/31/2019";
                    //exposureEndDateVal = "04/30/2020";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    inputSearch = "qa test 05";
                    managerName = "QA Test 05";
                    fundName = "Main QA Test 05";
                    fundid = 12;
                    //exposureNRowCountVal = "2";
                    //exposureStartDateVal = "03/31/2019";
                    //exposureEndDateVal = "04/30/2020";
                }

                // Search a Fund - Source = Manual (Albourne)
                SearchFundAction.Instance.InputNameToSearchFund(10, inputSearch)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(managerName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, managerName, sourceIcon)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(fundName, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model);
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Click 'Model Parameters' button (old name: User Input)
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput)
                                      .WaitForElementVisible(10, SearchFundPage.userInputPanel); Thread.Sleep(1000);

                //// Verify a red notification is shown at bottom next to the Run button if the fund does not have return data
                //verifyPoint = message == SearchFundAction.Instance.RedMessageFundDoesNotHaveReturnDataGetText(10);
                //verifyPoints.Add(summaryTC = "Verify a red notification is shown at bottom next to the Run button if the fund does not have return data:\n<br/> '" + message + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);

                // Click to expand User Input (section)
                SearchFundAction.Instance.ClickUserInputSubSection(10, SearchFundPage.dataStatus)
                                         .PageDownToScrollDownPage();

                // Click 'Upload File' button
                SearchFundAction.Instance.ClickLabelButton(10, "Upload File")
                                         .WaitForElementVisible(10, SearchFundPage.dialogPopup);

                #region Exposure (Upload invalid file)
                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Exposure");
                System.Windows.Forms.SendKeys.SendWait(@"{RIGHT}");

                // Upload file (txt file)
                string fileName = "txt_file.txt";
                string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tests\Documents\Empty files\");
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (txt file) (File Type: Exposure)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Txt) (File Type: Exposure): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile); Thread.Sleep(500);

                // Upload file (Excel file)
                fileName = "Excel_file.xlsx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Excel file) (File Type: Exposure)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Excel) (File Type: Exposure): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile); Thread.Sleep(1500);

                // Upload file (Word file)
                fileName = "Word_file.docx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Word file) (File Type: Exposure)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Word) (File Type: Exposure): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile); Thread.Sleep(500);
                #endregion

                #region Fund AUM (Upload invalid file)
                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Fund AUM");
                System.Windows.Forms.SendKeys.SendWait(@"{RIGHT}");

                // Upload file (txt file)
                fileName = "txt_file.txt";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (txt file) (File Type: Fund AUM)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Txt) (File Type: Fund AUM): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);

                // Upload file (Excel file)
                fileName = "Excel_file.xlsx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Excel file) (File Type: Fund AUM)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Excel) (File Type: Fund AUM): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);

                // Upload file (Word file)
                fileName = "Word_file.docx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Word file) (File Type: Fund AUM)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Word) (File Type: Fund AUM): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);
                #endregion

                #region Fund Returns (Upload invalid file)
                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Fund Returns");
                System.Windows.Forms.SendKeys.SendWait(@"{RIGHT}");

                // Upload file (txt file)
                fileName = "txt_file.txt";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (txt file) (File Type: Fund Returns)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Txt) (File Type: Fund Returns): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);

                // Upload file (Excel file)
                fileName = "Excel_file.xlsx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Excel file) (File Type: Fund Returns)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Excel) (File Type: Fund Returns): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);

                // Upload file (Word file)
                fileName = "Word_file.docx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Word file) (File Type: Fund Returns)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Word) (File Type: Fund Returns): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Exposure (Upload valid file) - Notes: Please turn on your VPN before running this test cases
                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Exposure");

                // Upload file (csv file - Exposure)
                fileName = "Upload_Exposure_2rows.csv";
                filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tests\Documents\CSV files\");
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Way 1: Press DownArrow key until element is visible
                // Fields Mapping - Select fields for 'Source' and 'Destination'
                SearchFundAction.Instance.ClickDestinationDropdown(10, 1)
                                         .WaitForAllElementsVisible(10, SearchFundPage.overlayDropdown)
                                         .PressDownArrowKeyUntilElementIsVisible(20);

                // Select item for 'Destination' dropdown
                SearchFundAction.Instance.WaitForAllElementsVisible(10, SearchFundPage.destinationDropdownItemSelect(1, SearchFundPage.date))
                                         .SelectItemInDestinationDropdown(10, 1, SearchFundPage.date)
                                         .ClickAndSelectItemInDestinationDropdown(10, 2, SearchFundPage.exposure)
                                         .ClickAndSelectItemInDestinationDropdown(10, 3, SearchFundPage.currency);
                                         //.ClickAndSelectItemInDestinationDropdown(10, 4, SearchFundPage.share_class_id);
                SearchFundAction.Instance.ClickDestinationDropdown(10, 4);
                System.Windows.Forms.SendKeys.SendWait(@"{UP}");
                SearchFundAction.Instance.PressEnterKeyboard();

                // Click Replace button
                SearchFundAction.Instance.ClickLabelButton(10, "Replace") //.ClickDoneButton();
                                         .WaitForAllElementsVisible(10, SearchFundPage.dialogWarning);
                SearchFundAction.Instance.ClickButtonInDialog(10, "Replace");

                // Verify a green toast message is shown after uploading a valid file  (File Type: Exposure)
                verifyPoint = "Success" == SearchFundAction.Instance.toastMessageAlertGetText(10, 1)
                            && "File uploaded successfully." == SearchFundAction.Instance.toastMessageAlertGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify a green toast message is shown after uploading a valid file (File Type: Exposure): (Success File uploaded successfully.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Wait for toast message is disappeared
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.toastMessage(1));

                #region Verify the database value in the table after uploading CSV file into DB
                const string db = "ks_model";
                string table = "manual_fund_exposure";
                //const string dataSource = "manual";
                string query = "SELECT fund_id, exposure, date, currency, share_class_id FROM " + db + "." + table + " WHERE fund_id=" + fundid + " AND _time_ >= timestamp '" + timestamp + "'";

                // Wait until all files are loaded
                System.Threading.Thread.Sleep(10000);

                // Send API Requests and Get API Responses (to retrieve data)
                var sendResponse = DatabaseConnection.GetDatalakeANSISQLQueries(db, query);
                List<JObject> jsonDB = JsonConvert.DeserializeObject<List<JObject>>(sendResponse.Content);

                // Convert csv (file) to json object (--> csv from file PATH)
                var convert = DatabaseConnection.ConvertCsvFileToJsonObject(filePath + fileName);
                List<JObject> jsFile = JsonConvert.DeserializeObject<List<JObject>>(convert);

                // Verify number of rows are loaded
                verifyPoint = jsonDB.Count == jsFile.Count;
                verifyPoints.Add(summaryTC = "Verify number of rows (Exposure) are loaded = " + jsFile.Count + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // JsonFile (csv file) - Rename Keys (field name) and change Date format in JObject
                DatabaseConnection.ChangeDateFormatInJObject(jsFile, "Year", "yyyy-MM-dd HH:mm:ss.fff");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Year", "date");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "ValExpos", "exposure");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Cur", "currency");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Share Cl ID", "share_class_id");
                DatabaseConnection.ChangeToDoubleTypeForValueInJObject(jsFile, "exposure");
                DatabaseConnection.AddFieldNameInJObject(jsFile, "fund_id", fundid);
                //DatabaseConnection.RemoveFieldNameInJObject(jsFile, "share_class_id");
                //DatabaseConnection.RemoveFieldNameInJObject(jsonDB, "share_class_id");

                // Sort (Properties) JSON file and JSON db
                var testFile = DatabaseConnection.SortPropertiesByName(jsFile);
                var testDB = DatabaseConnection.SortPropertiesByName(jsonDB);

                // Sort value in JObject
                var fileSort = testFile.OrderBy(p => (string?)p["exposure"]);
                var dbSort = testDB.OrderBy(p => (string?)p["exposure"]);

                // Verify data from csv file was loaded into DB correctly
                verifyPoint = SearchFundAction.Instance.CompareObjects(fileSort, dbSort);
                verifyPoints.Add(summaryTC = "Verify data (Exposure) from csv file was loaded into DB correctly", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Data is shown at "Data Status"
                // Click on (leftArrow icon / Back) button
                SearchFundAction.Instance.ClickBackButton(10); Thread.Sleep(1000);

                // Upload Source: Manual
                string? rowCount = null, startDate = null, endDate = null;
                /// Exposure Manual (index = 1)
                verifyPoint = (rowCount = "2") == SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.rowCounts)
                    && (startDate = "03/31/2019") == SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.startDate)
                    && (endDate = "04/30/2020") == SearchFundAction.Instance.ValueDataStatusGetText(10, 1, SearchFundPage.endDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Exposure: 'RowCount=" + rowCount + "'; 'StartDate=" + startDate + "'; 'EndDate=" + endDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
                #endregion

                #region Fund AUM (Upload valid file) - Notes: Please turn on your VPN before running this test cases
                // Click 'Upload File' button
                SearchFundAction.Instance.ClickLabelButton(10, "Upload File")
                                         .WaitForElementVisible(10, SearchFundPage.dialogPopup);

                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Fund AUM");

                // Upload file (csv file - Fund AUM)
                fileName = "Upload_FundAUM_2rows.csv";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Fields Mapping - Select fields for 'Source' and 'Destination'
                SearchFundAction.Instance.ClickDestinationDropdown(10, 1)
                                         .WaitForAllElementsVisible(10, SearchFundPage.overlayDropdown)
                                         .PressDownArrowKeyUntilElementIsVisible(20);

                // Select item for 'Destination' dropdown
                SearchFundAction.Instance.WaitForAllElementsVisible(10, SearchFundPage.destinationDropdownItemSelect(1, SearchFundPage.date))
                                         .SelectItemInDestinationDropdown(10, 1, SearchFundPage.date)
                                         .ClickAndSelectItemInDestinationDropdown(10, 2, SearchFundPage.aum)
                                         .ClickAndSelectItemInDestinationDropdown(10, 3, SearchFundPage.currency);
                                         //.ClickAndSelectItemInDestinationDropdown(10, 4, SearchFundPage.share_class_id);
                SearchFundAction.Instance.ClickDestinationDropdown(10, 4);
                System.Windows.Forms.SendKeys.SendWait(@"{UP}");
                SearchFundAction.Instance.PressEnterKeyboard();

                // Click Replace button
                SearchFundAction.Instance.ClickLabelButton(10, "Replace") //.ClickDoneButton();
                                         .WaitForAllElementsVisible(10, SearchFundPage.dialogWarning)
                                         .ClickButtonInDialog(10, "Replace");

                // Verify a green toast message is shown after uploading a valid file  (File Type: Fund AUM)
                verifyPoint = "Success" == SearchFundAction.Instance.toastMessageAlertGetText(10, 1)
                            && "File uploaded successfully." == SearchFundAction.Instance.toastMessageAlertGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify a green toast message is shown after uploading a valid file (File Type: Fund AUM): (Success File uploaded successfully.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Wait for toast message is disappeared
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.toastMessage(1));

                #region Verify the database value in the table after uploading CSV file into DB
                table = "manual_fund_aum";
                query = "SELECT fund_id, aum, date, currency, share_class_id FROM " + db + "." + table + " WHERE fund_id=" + fundid + " AND _time_ >= timestamp '" + timestamp + "'";

                // Wait until all files are loaded
                System.Threading.Thread.Sleep(11000);

                // Send API Requests and Get API Responses (to retrieve data)
                sendResponse = DatabaseConnection.GetDatalakeANSISQLQueries(db, query);
                jsonDB = JsonConvert.DeserializeObject<List<JObject>>(sendResponse.Content);

                // Convert csv (file) to json object (--> csv from file PATH)
                convert = DatabaseConnection.ConvertCsvFileToJsonObject(filePath + fileName);
                jsFile = JsonConvert.DeserializeObject<List<JObject>>(convert);

                // Verify number of rows are loaded
                verifyPoint = jsonDB.Count == jsFile.Count;
                verifyPoints.Add(summaryTC = "Verify number of rows (Fund AUM) are loaded = " + jsFile.Count + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // JsonFile (csv file) - Rename Keys (field name) and change Date format in JObject
                DatabaseConnection.ChangeDateFormatInJObject(jsFile, "Year", "yyyy-MM-dd HH:mm:ss.fff");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Year", "date");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Val", "aum");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Cur", "currency");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Share Cl ID", "share_class_id");
                DatabaseConnection.ChangeToIntegerTypeForValueInJObject(jsFile, "aum");
                DatabaseConnection.AddFieldNameInJObject(jsFile, "fund_id", fundid);
                //DatabaseConnection.RemoveFieldNameInJObject(jsFile, "share_class_id");
                //DatabaseConnection.RemoveFieldNameInJObject(jsonDB, "share_class_id");

                // Sort (Properties) JSON file and JSON db
                testFile = DatabaseConnection.SortPropertiesByName(jsFile);
                testDB = DatabaseConnection.SortPropertiesByName(jsonDB);

                // Sort value in JObject
                fileSort = testFile.OrderBy(p => (string?)p["aum"]);
                dbSort = testDB.OrderBy(p => (string?)p["aum"]);

                // Verify data from csv file was loaded into DB correctly
                verifyPoint = SearchFundAction.Instance.CompareObjects(fileSort, dbSort);
                verifyPoints.Add(summaryTC = "Verify data (Fund AUM) from csv file was loaded into DB correctly", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Data is shown at "Data Status"
                // Click on (leftArrow icon / Back) button
                SearchFundAction.Instance.ClickBackButton(10); Thread.Sleep(1000);

                // Upload Source: Manual
                /// Fund AUM Manual (index = 3)
                verifyPoint = (rowCount = "2") == SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.rowCounts)
                    && (startDate = "03/31/2020") == SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.startDate)
                    && (endDate = "04/30/2021") == SearchFundAction.Instance.ValueDataStatusGetText(10, 3, SearchFundPage.endDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Fund AUM: 'RowCount=" + rowCount + "'; 'StartDate=" + startDate + "'; 'EndDate=" + endDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
                #endregion

                #region Fund Returns (Upload valid file) - Notes: Please turn on your VPN before running this test cases
                // Click 'Upload File' button
                SearchFundAction.Instance.ClickLabelButton(10, "Upload File")
                                         .WaitForElementVisible(10, SearchFundPage.dialogPopup);

                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Fund Returns");

                // Upload file (csv file - Fund Returns)
                fileName = "Upload_FundReturns_2rows.csv";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Fields Mapping - Select fields for 'Source' and 'Destination'
                SearchFundAction.Instance.ClickDestinationDropdown(10, 1)
                                         .WaitForAllElementsVisible(10, SearchFundPage.overlayDropdown)
                                         .PressDownArrowKeyUntilElementIsVisible(20);

                // Select item for 'Destination' dropdown
                SearchFundAction.Instance.WaitForAllElementsVisible(10, SearchFundPage.destinationDropdownItemSelect(1, SearchFundPage.date))
                                         .SelectItemInDestinationDropdown(10, 1, SearchFundPage.date)
                                         .ClickAndSelectItemInDestinationDropdown(10, 2, SearchFundPage.net)
                                         .ClickAndSelectItemInDestinationDropdown(10, 3, SearchFundPage.gross)
                                         .ClickAndSelectItemInDestinationDropdown(10, 4, SearchFundPage.currency);

                SearchFundAction.Instance.ClickDestinationDropdown(10, 5);
                System.Windows.Forms.SendKeys.SendWait(@"{UP}");
                SearchFundAction.Instance.PressEnterKeyboard();

                // Click Replace button
                SearchFundAction.Instance.ClickLabelButton(10, "Replace") //.ClickDoneButton();
                                         .WaitForAllElementsVisible(10, SearchFundPage.dialogWarning)
                                         .ClickButtonInDialog(10, "Replace");

                // Verify a green toast message is shown after uploading a valid file  (File Type: Fund AUM)
                verifyPoint = "Success" == SearchFundAction.Instance.toastMessageAlertGetText(10, 1)
                            && "File uploaded successfully." == SearchFundAction.Instance.toastMessageAlertGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify a green toast message is shown after uploading a valid file (File Type: Fund Returns): (Success File uploaded successfully.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Wait for toast message is disappeared
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.toastMessage(1));

                #region Verify the database value in the table after uploading CSV file into DB
                table = "manual_fund_ror";
                query = "SELECT fund_id, gross, net, date, currency, share_class_id FROM " + db + "." + table + " WHERE fund_id=" + fundid + " AND _time_ >= timestamp '" + timestamp + "'";

                // Wait until all files are loaded
                System.Threading.Thread.Sleep(10000);

                // Send API Requests and Get API Responses (to retrieve data)
                sendResponse = DatabaseConnection.GetDatalakeANSISQLQueries(db, query);
                jsonDB = JsonConvert.DeserializeObject<List<JObject>>(sendResponse.Content);

                // Convert csv (file) to json object (--> csv from file PATH)
                convert = DatabaseConnection.ConvertCsvFileToJsonObject(filePath + fileName);
                jsFile = JsonConvert.DeserializeObject<List<JObject>>(convert); Thread.Sleep(2000);

                // Verify number of rows are loaded
                verifyPoint = jsonDB.Count == jsFile.Count;
                verifyPoints.Add(summaryTC = "Verify number of rows (Fund Returns) are loaded = " + jsFile.Count + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // JsonFile (csv file) - Rename Keys (field name) and change Date format in JObject
                DatabaseConnection.ChangeDateFormatInJObject(jsFile, "Year", "yyyy-MM-dd HH:mm:ss.fff");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Year", "date");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "ValNet", "net");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "ValGross", "gross");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Cur", "currency");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Share Cl ID", "share_class_id");
                DatabaseConnection.ChangeToDoubleTypeForValueInJObject(jsFile, "net");
                DatabaseConnection.ChangeToDoubleTypeForValueInJObject(jsFile, "gross");
                DatabaseConnection.AddFieldNameInJObject(jsFile, "fund_id", fundid);
                //DatabaseConnection.RemoveFieldNameInJObject(jsFile, "share_class_id");
                //DatabaseConnection.RemoveFieldNameInJObject(jsonDB, "share_class_id");

                // Sort (Properties) JSON file and JSON db
                testFile = DatabaseConnection.SortPropertiesByName(jsFile);
                testDB = DatabaseConnection.SortPropertiesByName(jsonDB);

                // Sort value in JObject
                fileSort = testFile.OrderBy(p => (string?)p["net"]);
                dbSort = testDB.OrderBy(p => (string?)p["net"]);

                // Verify data from csv file was loaded into DB correctly
                verifyPoint = SearchFundAction.Instance.CompareObjects(fileSort, dbSort);
                verifyPoints.Add(summaryTC = "Verify data (Fund Returns) from csv file was loaded into DB correctly", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Data is shown at "Data Status"
                // Click on (leftArrow icon / Back) button
                Thread.Sleep(2000);
                SearchFundAction.Instance.ClickBackButton(10); Thread.Sleep(1000);

                // Upload Source: Manual
                /// Fund Returns Manual (index = 2)
                verifyPoint = (rowCount = "2") == SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.rowCounts)
                    && (startDate = "07/31/2009") == SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.startDate)
                    && (endDate = "12/31/2021") == SearchFundAction.Instance.ValueDataStatusGetText(10, 2, SearchFundPage.endDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Fund Returns: 'RowCount=" + rowCount + "'; 'StartDate=" + startDate + "'; 'EndDate=" + endDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Test, Category("Regression Testing")]
        public void TC002_UploadFile_PrivateFund()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string sourceIcon = "M";
            const string currency = "USD";
            DateTime timestampOrg = DateTime.UtcNow.AddDays(-1);
            string timestamp = timestampOrg.ToString("yyyy-MM-dd 00:00:00.000"); //DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string videoFileName = "UploadFileTestTC002";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Upload File Test - TC002");
            try
            {
                // Start recording video
                Driver.StartVideoRecord(videoFileName);

                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                string? inputSearch = null, cambridgeFund = null, managerName = null, asOfDate = null; int? id = null;
                if (urlInstance.Contains("lab"))
                {
                    inputSearch = "priman f0";
                    cambridgeFund = "PriMan F03";
                    managerName = "PriMan F03";
                    id = 11; // (manager id)
                    asOfDate = "09-30-2022";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    inputSearch = "priman fun";
                    cambridgeFund = "Firm 01 of PriMan FunSta 01";
                    managerName = "PriMan FunSta 01";
                    id = 1; // (manager id)
                    asOfDate = "09-30-2022";
                }

                /// Search a Cambridge Fund (KS-455 the Firm should be searched in the search bar)
                // Search a Fund - Source = Manual Cambridge/Private
                SearchFundAction.Instance.InputNameToSearchFund(10, inputSearch)
                                         .WaitForElementVisible(10, SearchFundPage.fundNameReturnOfResultsWithItemSource(cambridgeFund, sourceIcon))
                                         .ClickFundNameReturnOfResults(10, cambridgeFund, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(1000);

                // Check if the spinner loading icon is shown then wait for it load done
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.loadingSpinner);
                }

                // Click 'Model' menu
                NavigationAction.Instance.ClickPageNames(10, SearchFundPage.model);
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.labelButton(SearchFundPage.userInput));

                // Click 'Model Parameters' button (old name: User Input)
                GeneralAction.Instance.ClickButtonLabel(10, SearchFundPage.userInput)
                                      .WaitForElementVisible(10, SearchFundPage.userInputPanel); Thread.Sleep(1000);

                // Click 'Upload File' button
                SearchFundAction.Instance.ClickLabelButton(10, "Upload File")
                                         .WaitForElementVisible(10, SearchFundPage.dialogPopup); Thread.Sleep(500);

                #region Deal Information (Upload invalid file)
                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Deal Information");
                System.Windows.Forms.SendKeys.SendWait(@"{RIGHT}");

                // Upload file (txt file)
                string fileName = "txt_file.txt";
                string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tests\Documents\Empty files\");
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (txt file) (File Type: Deal Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Txt) (File Type: Deal Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile); Thread.Sleep(500);

                // Upload file (Excel file)
                fileName = "Excel_file.xlsx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Excel file) (File Type: Deal Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Excel) (File Type: Deal Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile); Thread.Sleep(1500);

                // Upload file (Word file)
                fileName = "Word_file.docx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Word file) (File Type: Deal Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Word) (File Type: Deal Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile); Thread.Sleep(500);
                #endregion

                #region Fund Information (Upload invalid file)
                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Fund Information");
                System.Windows.Forms.SendKeys.SendWait(@"{RIGHT}");

                // Upload file (txt file)
                fileName = "txt_file.txt";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (txt file) (File Type: Fund Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Txt) (File Type: Fund Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);

                // Upload file (Excel file)
                fileName = "Excel_file.xlsx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Excel file) (File Type: Fund Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Excel) (File Type: Fund Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);

                // Upload file (Word file)
                fileName = "Word_file.docx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Word file) (File Type: Fund Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Word) (File Type: Fund Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);
                #endregion

                #region Cash Flow Information (Upload invalid file)
                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Cash Flow Information");
                System.Windows.Forms.SendKeys.SendWait(@"{RIGHT}");

                // Upload file (txt file)
                fileName = "txt_file.txt";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (txt file) (File Type: Cash Flow Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Txt) (File Type: Cash Flow Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);

                // Upload file (Excel file)
                fileName = "Excel_file.xlsx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Excel file) (File Type: Cash Flow Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Excel) (File Type: Cash Flow Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);

                // Upload file (Word file)
                fileName = "Word_file.docx";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName);

                // Verify a red toast message is shown after uploading an invalid file (Word file) (File Type: Cash Flow Information)
                verifyPoint = fileName + ": Invalid file type," == SearchFundAction.Instance.ToastMessageInvalidFileGetText(10)
                           && "allowed file types: .csv." == SearchFundAction.Instance.ToastMessageInvalidFileDetailGetText(10);
                verifyPoints.Add(summaryTC = "Verify a red toast message is shown after uploading an invalid file (Word) (File Type: Cash Flow Information): (" + fileName + ": Invalid file type, allowed file types: .csv.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click (x) to close the red toast message
                SearchFundAction.Instance.ClickCloseToastMessageButton(10)
                                         .WaitForElementInvisible(10, SearchFundPage.toastMessageInvalidFile);
                #endregion

                #region Deal Information (Upload valid file) - Notes: Please turn on your VPN before running this test cases
                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Deal Information");

                // Upload file (csv file - Deal Information)
                fileName = "deal_information.csv";
                filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Tests\Documents\CSV files\");
                SearchFundAction.Instance.UploadFileInput(filePath + fileName) // Upload file (csv file - Deal Information)
                                         .ClickAndSelectItemInDropdown(10, "Currency", currency)
                                         .InputAsOfDate(10, asOfDate);

                // Click 'Destination' dropdown at row (x)
                SearchFundAction.Instance.ClickDestinationDropdown(10, 1)
                                         .WaitForAllElementsVisible(10, SearchFundPage.overlayDropdown)
                                         .PressDownArrowKeyUntilElementIsVisible(20);

                // Select item for 'Destination' dropdown
                SearchFundAction.Instance.WaitForAllElementsVisible(10, SearchFundPage.destinationDropdownItemSelect(1, SearchFundPage.company_name))
                                         .SelectItemInDestinationDropdown(10, 1, SearchFundPage.company_name)
                                         .ClickAndSelectItemInDestinationDropdown(10, 2, SearchFundPage.fund)
                                         .ClickAndSelectItemInDestinationDropdown(10, 3, SearchFundPage.entry_date)
                                         .ClickAndSelectItemInDestinationDropdown(10, 4, SearchFundPage.exit_date)
                                         .ClickAndSelectItemInDestinationDropdown(10, 5, SearchFundPage.status)
                                         .ClickAndSelectItemInDestinationDropdown(10, 6, SearchFundPage.gross_irr)
                                         .ClickAndSelectItemInDestinationDropdown(10, 7, SearchFundPage.invested_capital)
                                         .ClickAndSelectItemInDestinationDropdown(10, 8, SearchFundPage.realized_capital)
                                         .ClickAndSelectItemInDestinationDropdown(10, 9, SearchFundPage.unrealized_fmv)
                                         .ClickAndSelectItemInDestinationDropdown(10, 10, SearchFundPage.attribution_category_1)
                                         .ClickAndSelectItemInDestinationDropdown(10, 11, SearchFundPage.attribution_category_2)
                                         .ClickAndSelectItemInDestinationDropdown(10, 12, SearchFundPage.attribution_category_3)
                                         .ClickAndSelectItemInDestinationDropdown(10, 13, SearchFundPage.custom_weight);
                //.ClickAndSelectItemInDestinationDropdown(10, 14, "manager") // --> No map
                //.ClickAndSelectItemInDestinationDropdown(10, 15, "currency") // --> No map
                //.ClickAndSelectItemInDestinationDropdown(10, 16, "data_as_of_date") // --> No map

                // Click Done button
                SearchFundAction.Instance.ClickLabelButton(10, "Done");

                // Verify a green toast message is shown after uploading a valid file
                verifyPoint = "Success" == SearchFundAction.Instance.toastMessageAlertGetText(10, 1)
                            && "File uploaded successfully." == SearchFundAction.Instance.toastMessageAlertGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify a green toast message is shown after uploading a valid file (File Type: Deal Information): (Success File uploaded successfully.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Wait for toast message is disappeared
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.toastMessage(1)); Thread.Sleep(3000);

                #region Verify the database value in the table after uploading CSV file into DB
                const string db = "ks_model";
                string table = "private_deal_information";
                DateTime convert_as_of_date = DateTime.ParseExact(asOfDate, "MM-dd-yyyy", null);
                string data_as_of_date = convert_as_of_date.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string selectFields = "manager_id, company_name, fund, entry_date, exit_date, status, gross_irr, invested_capital, realized_capital, unrealized_fmv, attribution_category_1, attribution_category_2, attribution_category_3, custom_weight";
                string query = "SELECT " + selectFields + " FROM " + db + "." + table + " WHERE manager_id='" + id + "' AND _time_>= timestamp '" + timestamp + "'";

                // Wait until all files are loaded
                System.Threading.Thread.Sleep(23000);

                // Send API Requests and Get API Responses (to retrieve data)
                var sendResponse = DatabaseConnection.GetDatalakeANSISQLQueries(db, query);
                List<JObject> jsonDB = JsonConvert.DeserializeObject<List<JObject>>(sendResponse.Content);

                // Convert csv (file) to json object (--> csv from file PATH)
                var convert = DatabaseConnection.ConvertCsvFileToJsonObject(filePath + fileName);
                List<JObject> jsFile = JsonConvert.DeserializeObject<List<JObject>>(convert);

                // Verify number of rows are loaded
                verifyPoint = jsonDB.Count == jsFile.Count;
                verifyPoints.Add(summaryTC = "Verify number of rows (Deal Information) are loaded = " + jsFile.Count + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // JsonFile (csv file) - Rename Keys (field name) and change Date format in JObject
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Attribution Category 1", "attribution_category_1");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Attribution Category 2", "attribution_category_2");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Attribution Category 3", "attribution_category_3");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Company Name", "company_name");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Currency", "currency");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Custom Weight", "custom_weight");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Data as of Date", "data_as_of_date");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Entry Date", "entry_date");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Exit Date", "exit_date");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Fund", "fund");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Gross IRR", "gross_irr");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Invested Capital", "invested_capital");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Manager", "manager");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Realized Capital", "realized_capital");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Status", "status");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Unrealized FMV", "unrealized_fmv");
                DatabaseConnection.RemoveWhiteSpaceBeginAndEndValuesInJObject(jsFile, "attribution_category_1");
                DatabaseConnection.RemoveWhiteSpaceBeginAndEndValuesInJObject(jsFile, "company_name");
                DatabaseConnection.ReplaceTextValueInJObject(jsFile, "custom_weight", "", null);
                DatabaseConnection.ReplaceTextValueInJObject(jsFile, "data_as_of_date", "6/1/2020", data_as_of_date);
                DatabaseConnection.ChangeDateFormatInJObject(jsFile, "data_as_of_date", "yyyy-MM-dd HH:mm:ss.fff");
                DatabaseConnection.ChangeDateFormatWithNullInJObject(jsFile, "entry_date", "yyyy-MM-dd HH:mm:ss.fff", null, null);
                DatabaseConnection.ChangeDateFormatWithNullInJObject(jsFile, "exit_date", "yyyy-MM-dd HH:mm:ss.fff", null, null);
                DatabaseConnection.ChangeValuesInJObject(jsFile, "manager", managerName); // cambridgeFund
                DatabaseConnection.ReplaceContainTextValueInJObject(jsFile, "gross_irr", "%", ""); // --> Remove % sign by replacing with ""
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, "gross_irr", "0.##", null, null);
                DatabaseConnection.ChangeToIntegerTypeForValueInJObject(jsFile, "invested_capital");
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, "realized_capital", "0.##", " -   ", null);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, "unrealized_fmv", "0.##", " -   ", null);
                DatabaseConnection.AddFieldNameInJObject(jsFile, "manager_id", "" + id + "");
                /// JsonFile (csv file) - Rename Keys(field name) and change Date format in JObject
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "manager");
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "currency");
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "data_as_of_date");
                //DatabaseConnection.RemoveFieldNameInJObject(jsFile, "manager_id");
                //DatabaseConnection.RemoveFieldNameInJObject(jsonDB, "manager_id");

                // Sort (Properties) JSON file and JSON db
                var testFile = DatabaseConnection.SortPropertiesByName(jsFile);
                var testDB = DatabaseConnection.SortPropertiesByName(jsonDB);

                // Sort value in JObject
                var fileSort = testFile.OrderBy(p => (string?)p["company_name"]);
                var dbSort = testDB.OrderBy(p => (string?)p["company_name"]);

                // Verify data from csv file was loaded into DB correctly
                verifyPoint = SearchFundAction.Instance.CompareJObjectsToString(fileSort, dbSort);
                verifyPoints.Add(summaryTC = "Verify data (Deal Information) from csv file was loaded into DB correctly", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Data is shown at "Data Status"
                // Click on (leftArrow icon / Back) button
                SearchFundAction.Instance.ClickBackButton(10); Thread.Sleep(1000);

                string rowCount, uploadDate;
                // Upload Source: Manual
                /// Deal Information
                verifyPoint = (rowCount = "23") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 1, SearchFundPage.rowCount)
                    && (asOfDate = "09/30/2022") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 1, SearchFundPage.asOfDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Deal Information: 'RowCount=" + rowCount + "'; 'AsOfDate=" + asOfDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
                #endregion

                #region Fund Information (Upload valid file) - Notes: Please turn on your VPN before running this test cases
                // Click 'Upload File' button
                SearchFundAction.Instance.ClickLabelButton(10, "Upload File")
                                         .WaitForElementVisible(10, SearchFundPage.dialogPopup);

                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Fund Information");

                // Upload file (csv file - Deal Information)
                fileName = "fund_information.csv";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName) // Upload file (csv file - Fund Information)
                                         .ClickAndSelectItemInDropdown(10, "Currency", currency)
                                         .InputAsOfDate(10, asOfDate= "09-30-2022");

                // Click 'Destination' dropdown at row (x)
                SearchFundAction.Instance.ClickDestinationDropdown(10, 1)
                                         .WaitForAllElementsVisible(10, SearchFundPage.overlayDropdown)
                                         .PressDownArrowKeyUntilElementIsVisible(20);

                // Select item for 'Destination' dropdown
                SearchFundAction.Instance.WaitForAllElementsVisible(10, SearchFundPage.destinationDropdownItemSelect(1, SearchFundPage.fund_name))
                                         .SelectItemInDestinationDropdown(10, 1, SearchFundPage.fund_name)
                                         .ClickAndSelectItemInDestinationDropdown(10, 2, SearchFundPage.fund_size)
                                         .ClickAndSelectItemInDestinationDropdown(10, 3, SearchFundPage.vintage_year)
                                         .ClickAndSelectItemInDestinationDropdown(10, 4, SearchFundPage.invested_capital)
                                         .ClickAndSelectItemInDestinationDropdown(10, 5, SearchFundPage.realized)
                                         .ClickAndSelectItemInDestinationDropdown(10, 6, SearchFundPage.unrealized_current_nav)
                                         .ClickAndSelectItemInDestinationDropdown(10, 7, SearchFundPage.gross_irr)
                                         .ClickAndSelectItemInDestinationDropdown(10, 8, SearchFundPage.net_irr)
                                         .ClickAndSelectItemInDestinationDropdown(10, 9, SearchFundPage.gross_tvpi)
                                         .ClickAndSelectItemInDestinationDropdown(10, 10, SearchFundPage.net_tvpi);
                //.ClickAndSelectItemInDestinationDropdown(10, 11, "manager") // --> No map
                //.ClickAndSelectItemInDestinationDropdown(10, 12, "Currency") // --> No map
                //.ClickAndSelectItemInDestinationDropdown(10, 13, "data_as_of_date") // --> No map

                // Click Done button
                SearchFundAction.Instance.ClickLabelButton(10, "Done");

                // Verify a green toast message is shown after uploading a valid file
                verifyPoint = "Success" == SearchFundAction.Instance.toastMessageAlertGetText(10, 1)
                            && "File uploaded successfully." == SearchFundAction.Instance.toastMessageAlertGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify a green toast message is shown after uploading a valid file (File Type: Fund Information): (Success File uploaded successfully.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Wait for toast message is disappeared
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.toastMessage(1));

                #region Verify the database value in the table after uploading CSV file into DB
                table = "private_fund_information";
                convert_as_of_date = DateTime.ParseExact(asOfDate, "MM-dd-yyyy", null);
                data_as_of_date = convert_as_of_date.ToString("yyyy-MM-dd HH:mm:ss.fff");
                selectFields = "manager_id, fund_name, fund_size, vintage_year, invested_capital, realized, unrealized_current_nav, gross_irr, net_irr, gross_tvpi, net_tvpi";
                query = "SELECT " + selectFields + " FROM " + db + "." + table + " WHERE manager_id='" + id + "' AND _time_>= timestamp '" + timestamp + "'";

                // Wait until all files are loaded
                System.Threading.Thread.Sleep(10000);

                // Send API Requests and Get API Responses (to retrieve data)
                sendResponse = DatabaseConnection.GetDatalakeANSISQLQueries(db, query);
                jsonDB = JsonConvert.DeserializeObject<List<JObject>>(sendResponse.Content);

                // Convert csv (file) to json object (--> csv from file PATH)
                convert = DatabaseConnection.ConvertCsvFileToJsonObject(filePath + fileName);
                jsFile = JsonConvert.DeserializeObject<List<JObject>>(convert);

                // Verify number of rows are loaded into DB
                verifyPoint = jsonDB.Count == jsFile.Count;
                verifyPoints.Add(summaryTC = "Verify number of rows (Fund Information) are loaded = " + jsFile.Count + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // JsonFile (csv file) - Rename Keys (field name) and change Date format in JObject
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Fund Name", SearchFundPage.fund_name);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Fund Size", SearchFundPage.fund_size);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Vintage Year", SearchFundPage.vintage_year);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Invested Capital", SearchFundPage.invested_capital);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Realized", SearchFundPage.realized);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Unrealized (Current NAV)", SearchFundPage.unrealized_current_nav);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Gross IRR", SearchFundPage.gross_irr);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Net IRR", SearchFundPage.net_irr);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Gross TVPI", SearchFundPage.gross_tvpi);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Net TVPI", SearchFundPage.net_tvpi);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Manager", "manager");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Currency", SearchFundPage.currency);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Data as of Date", SearchFundPage.data_as_of_date);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.fund_size, "0.##", null, null);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.vintage_year, "0.##", null, null);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.invested_capital, "0.##", null, null);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.realized, "0.##", null, null);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.unrealized_current_nav, "0.##", null, null);
                DatabaseConnection.ReplaceContainTextValueInJObject(jsFile, SearchFundPage.gross_irr, "%", ""); // --> Remove % sign by replacing with ""
                DatabaseConnection.ReplaceContainTextValueInJObject(jsFile, SearchFundPage.net_irr, "%", ""); // --> Remove % sign by replacing with ""
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.gross_irr, "0.##", null, null);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.net_irr, "0.##", null, null);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.gross_tvpi, "0.##", null, null);
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.net_tvpi, "0.##", null, null);
                DatabaseConnection.ChangeValuesInJObject(jsFile, "manager", managerName); // cambridgeFund
                DatabaseConnection.ReplaceTextValueInJObject(jsFile, SearchFundPage.currency, "USD", currency);
                DatabaseConnection.ReplaceTextValueInJObject(jsFile, SearchFundPage.data_as_of_date, "6/1/2020", data_as_of_date);
                DatabaseConnection.AddFieldNameInJObject(jsFile, "manager_id", "" + id + "");
                /// JsonFile (csv file) - Rename Keys(field name) and change Date format in JObject
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "manager");
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "currency");
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "data_as_of_date");
                //DatabaseConnection.RemoveFieldNameInJObject(jsFile, "manager_id");
                //DatabaseConnection.RemoveFieldNameInJObject(jsonDB, "manager_id");

                // Sort (Properties) JSON file and JSON db
                testFile = DatabaseConnection.SortPropertiesByName(jsFile);
                testDB = DatabaseConnection.SortPropertiesByName(jsonDB);

                // Sort value in JObject
                //DatabaseConnection.RemoveFieldNameInJObject(jsonDB, "manager_source");
                fileSort = testFile.OrderBy(p => (string?)p["fund_name"]);
                dbSort = testDB.OrderBy(p => (string?)p["fund_name"]);

                // Verify data from csv file was loaded into DB correctly
                verifyPoint = SearchFundAction.Instance.CompareJObjectsToString(fileSort, dbSort);
                verifyPoints.Add(summaryTC = "Verify data (Fund Information) from csv file was loaded into DB correctly", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Data is shown at "Data Status"
                // Click on (leftArrow icon / Back) button
                SearchFundAction.Instance.ClickBackButton(10); Thread.Sleep(1000);

                // Upload Source: Manual
                /// Fund Information
                verifyPoint = (rowCount = "2") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 2, SearchFundPage.rowCount)
                    && (asOfDate = "09/30/2022") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 2, SearchFundPage.asOfDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Fund Information: 'RowCount=" + rowCount + "'; 'AsOfDate=" + asOfDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
                #endregion

                #region Cash Flow Information (Upload valid file) - Notes: Please turn on your VPN before running this test cases
                // Click 'Upload File' button
                SearchFundAction.Instance.ClickLabelButton(10, "Upload File")
                                         .WaitForElementVisible(10, SearchFundPage.dialogPopup);

                // Input data for File Type (dropdown)
                SearchFundAction.Instance.ClickAndSelectItemInDropdown(10, SearchFundPage.fileType, "Cash Flow Information");

                // Upload file (csv file - Cash Flow Information)
                fileName = "cashflow_information.csv";
                SearchFundAction.Instance.UploadFileInput(filePath + fileName) // Upload file (csv file - Cash Flow Information)
                                         .ClickAndSelectItemInDropdown(10, "Currency", currency)
                                         .InputAsOfDate(10, asOfDate = "09-30-2022");

                // Click 'Destination' dropdown at row (x)
                SearchFundAction.Instance.ClickDestinationDropdown(10, 1)
                                         .WaitForAllElementsVisible(10, SearchFundPage.overlayDropdown)
                                         .PressDownArrowKeyUntilElementIsVisible(20);

                // Select item for 'Destination' dropdown
                SearchFundAction.Instance.WaitForAllElementsVisible(10, SearchFundPage.destinationDropdownItemSelect(1, SearchFundPage.date))
                                         .SelectItemInDestinationDropdown(10, 1, SearchFundPage.date)
                                         .ClickAndSelectItemInDestinationDropdown(10, 2, SearchFundPage.fund)
                                         .ClickAndSelectItemInDestinationDropdown(10, 3, SearchFundPage.contribution)
                                         .ClickAndSelectItemInDestinationDropdown(10, 4, SearchFundPage.distribution);
                //.ClickAndSelectItemInDestinationDropdown(10, 5, "manager") // --> No map
                //.ClickAndSelectItemInDestinationDropdown(10, 6, "currency") // --> No map
                //.ClickAndSelectItemInDestinationDropdown(10, 7, "data_as_of_date") // --> No map

                // Click Done button
                SearchFundAction.Instance.ClickLabelButton(10, "Done");

                // Verify a green toast message is shown after uploading a valid file
                verifyPoint = "Success" == SearchFundAction.Instance.toastMessageAlertGetText(10, 1)
                            && "File uploaded successfully." == SearchFundAction.Instance.toastMessageAlertGetText(10, 2);
                verifyPoints.Add(summaryTC = "Verify a green toast message is shown after uploading a valid file (File Type: Cash Flow Information): (Success File uploaded successfully.)", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Wait for toast message is disappeared
                SearchFundAction.Instance.WaitForElementInvisible(10, SearchFundPage.toastMessage(1));

                #region Verify the database value in the table after uploading CSV file into DB
                table = "private_cash_flow_information";
                convert_as_of_date = DateTime.ParseExact(asOfDate, "MM-dd-yyyy", null);
                data_as_of_date = convert_as_of_date.ToString("yyyy-MM-dd HH:mm:ss.fff");
                selectFields = "manager_id, date, fund, contribution, distribution";
                query = "SELECT " + selectFields + " FROM " + db + "." + table + " WHERE manager_id='" + id + "' AND _time_>= timestamp '" + timestamp + "'";

                // Wait until all files are loaded
                System.Threading.Thread.Sleep(10000);

                // Send API Requests and Get API Responses (to retrieve data)
                sendResponse = DatabaseConnection.GetDatalakeANSISQLQueries(db, query);
                jsonDB = JsonConvert.DeserializeObject<List<JObject>>(sendResponse.Content); // JObject

                // Convert csv (file) to json object (--> csv from file PATH)
                convert = DatabaseConnection.ConvertCsvFileToJsonObjectContainsQuoted(filePath + fileName);
                jsFile = JsonConvert.DeserializeObject<List<JObject>>(convert);

                // Verify number of rows are loaded into DB
                verifyPoint = jsonDB.Count == jsFile.Count;
                verifyPoints.Add(summaryTC = "Verify number of rows (Cash Flow Information) are loaded = " + jsFile.Count + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // JsonFile (csv file) - Rename Keys (field name) and change Date format in JObject
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Date", SearchFundPage.date);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Fund", SearchFundPage.fund);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Contribution", SearchFundPage.contribution);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Distribution", SearchFundPage.distribution);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Manager", "manager");
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Currency", SearchFundPage.currency);
                DatabaseConnection.ChangeFieldNameInJObject(jsFile, "Data as of Date", SearchFundPage.data_as_of_date);

                DatabaseConnection.ChangeDateFormatWithNullInJObject(jsFile, SearchFundPage.date, "yyyy-MM-dd HH:mm:ss.fff", null, null);
                DatabaseConnection.RemoveWhiteSpaceBeginAndEndValuesInJObject(jsFile, SearchFundPage.fund);

                DatabaseConnection.ReplaceContainTextValueInJObject(jsFile, SearchFundPage.contribution, ",", ""); // --> Remove , sign by replacing with ""
                DatabaseConnection.ReplaceContainTextValueInJObject(jsFile, SearchFundPage.contribution, @"""", ""); // --> Remove "" sign (double quotes) by replacing with ""
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.contribution, "0.##", " -   ", null);
                DatabaseConnection.ReplaceContainTextValueInJObject(jsFile, SearchFundPage.distribution, ",", ""); // --> Remove , sign by replacing with ""
                DatabaseConnection.ReplaceContainTextValueInJObject(jsFile, SearchFundPage.distribution, @"""", ""); // --> Remove "" sign (double quotes) by replacing with ""
                DatabaseConnection.ChangeToFormatDoubleAndIntegerWithNullValueInJObject(jsFile, SearchFundPage.distribution, "0.##", " -   ", null);

                DatabaseConnection.ChangeValuesInJObject(jsFile, "manager", managerName); // cambridgeFund
                DatabaseConnection.ChangeValuesInJObject(jsFile, SearchFundPage.currency, currency);
                DatabaseConnection.ChangeValuesInJObject(jsFile, SearchFundPage.data_as_of_date, data_as_of_date);
                DatabaseConnection.AddFieldNameInJObject(jsFile, "manager_id", "" + id + "");
                
                /// JsonFile (csv file) - Rename Keys(field name) and change Date format in JObject
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "manager");
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "currency");
                DatabaseConnection.RemoveFieldNameInJObject(jsFile, "data_as_of_date");
                //DatabaseConnection.RemoveFieldNameInJObject(jsFile, "manager_id");
                //DatabaseConnection.RemoveFieldNameInJObject(jsonDB, "manager_id");

                // Sort (Properties) JSON file and JSON db
                testFile = DatabaseConnection.SortPropertiesByName(jsFile);
                testDB = DatabaseConnection.SortPropertiesByName(jsonDB);

                // Sort value in JObject
                fileSort = testFile.OrderBy(p => (string?)p[SearchFundPage.date]).OrderBy(o => (string?)o.Property(SearchFundPage.contribution).Value == null).ThenBy(o => (string?)o.Property(SearchFundPage.contribution).Value);
                dbSort = testDB.OrderBy(p => (string?)p[SearchFundPage.date]).OrderBy(o => (string?)o.Property(SearchFundPage.contribution).Value == null).ThenBy(o => (string?)o.Property(SearchFundPage.contribution).Value);

                // Verify data from csv file was loaded into DB correctly
                verifyPoint = SearchFundAction.Instance.CompareJObjectsToString(fileSort, dbSort); // CompareObjects CompareJObjectsToString
                verifyPoints.Add(summaryTC = "Verify data (Cash Flow Information) from csv file was loaded into DB correctly", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Verify Data is shown at "Data Status"
                // Click on (leftArrow icon / Back) button
                SearchFundAction.Instance.ClickBackButton(10); Thread.Sleep(1000);

                // Upload Source: Manual
                /// Cash Flow Information
                verifyPoint = (rowCount = "37") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 3, SearchFundPage.rowCount)
                    && (asOfDate = "09/30/2022") == SearchFundAction.Instance.ValueDataStatusPrivateGetText(10, 3, SearchFundPage.asOfDate);
                verifyPoints.Add(summaryTC = "Verify data in 'Data Status' - 'Upload Source: Manual' - Cash Flow Information: 'RowCount=" + rowCount + "'; 'AsOfDate=" + asOfDate + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
                #endregion

                // Stop recording video
                Driver.StopVideoRecord();

                // Delete video file
                Driver.DeleteFilesContainsName(Path.GetFullPath(@"../../../../../TestResults/"), videoFileName);
            }
            catch (Exception exception)
            {
                // Stop recording video
                Driver.StopVideoRecord();

                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }
    }
}
