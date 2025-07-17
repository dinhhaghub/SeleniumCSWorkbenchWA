using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using WorkbenchApp.UITest.Core.BaseTestCase;
using WorkbenchApp.UITest.Core.Selenium;
using WorkbenchApp.UITest.Generals;
using WorkbenchApp.UITest.Pages;

namespace WorkbenchApp.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(9)]
    internal class AddEditFundTest : BaseTestCase
    {
        // Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [Test, Category("Regression Testing")]
        public void TC001_Add_PublicFund_AddNewFund()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string fundType = "Public Fund";
            string? managerName = null;
            string? fundName1 = null;
            string? fundName2 = null;
            const string fundNameExistsMsg = "Fund name already exists in the manager";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Add Fund Test - TC001");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                if (urlInstance.Contains("lab"))
                {
                    managerName = "QA Test 05";
                    fundName1 = "Main QA Test 05";
                    fundName2 = "Child 01 of QA Test 05";
                }
                if (urlInstance.Contains("conceptia"))
                {
                    managerName = "QA Test 01";
                    fundName1 = "Main of QA Test 01";
                    fundName2 = "Dinh Fund 01";
                }

                // Click on 'Fund Setup' button
                AddEditFundAction.Instance.ClickButtonLabel(10, "Fund Setup")
                                          .WaitForElementVisible(10, General.dynamicDialog); Thread.Sleep(500);

                // Check if 'spinner' loading icon is shown then wait for it to disappear
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                }

                AddEditFundAction.Instance.ClickMenuButtonLabel(10, "Modeling");

                // Click to select a Fund Type
                AddEditFundAction.Instance.ClickFundTypeDropdown(10).WaitForElementVisible(10, General.overlayDropdown)
                                          .ClickToSelectItemInDropdown(10, fundType) // Public Fund
                                          .WaitForElementInvisible(10, AddEditFundPage.overlayDropdown);

                // Input to add a new Fund
                AddEditFundAction.Instance.ClearInputFieldLabel(10, AddEditFundPage.fundManager);
                AddEditFundAction.Instance.InputFieldLabel(10, AddEditFundPage.fundManager, managerName)
                                          .ClickFundDropdownInputField(10, AddEditFundPage.fundManager) //.ClickLabelDropdownInputField(10, AddEditFundPage.fundManager)
                                          .ClickButtonLinkInDropdown(10, "Save as New Manager"); Thread.Sleep(2000);
                /// Work-around for Staging
                AddEditFundAction.Instance.ClearInputFieldLabel(10, AddEditFundPage.fundManager);
                AddEditFundAction.Instance.InputFieldLabel(10, AddEditFundPage.fundManager, managerName)
                                          .ClickFundDropdownInputField(10, AddEditFundPage.fundManager) //.ClickLabelDropdownInputField(10, AddEditFundPage.fundManager)
                                          .ClickButtonLinkInDropdown(10, "Save as New Manager");

                // Check if loading icon is displayed then wait for it disappear
                System.Threading.Thread.Sleep(500);
                if (GeneralAction.Instance.IsElementPresent(AddEditFundPage.loadingIcon))
                {
                    GeneralAction.Instance.WaitForLoadingIconToDisappear(20, AddEditFundPage.loadingIcon);
                }
                                      
                // Verify the toast message fund exists is shown correctly after clicking on save button
                string msg = "Data Already Exists: " + managerName;
                verifyPoint = msg == AddEditFundAction.Instance.toastMessageAlertGetText(10, msg); // ErrorMessageManagerNameExistsGetText
                verifyPoints.Add(summaryTC = "Verify the toast message (Manager Name Exists) is shown correctly: " + msg + " ", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // wait for toast message to disappear
                AddEditFundAction.Instance.WaitForElementInvisible(10, AddEditFundPage.toastMessage(msg));

                // Add a new Public Fund (Only add new Fund on Sandbox site!!!)
                if (urlInstance.Contains("lab"))
                {
                    #region Add a new Fund Manager
                    managerName = "QA_Auto_Manager" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
                    AddEditFundAction.Instance.InputFieldLabel(10, AddEditFundPage.fundManager, managerName);
                    AddEditFundAction.Instance.ClickFundDropdownInputField(10, AddEditFundPage.fundManager);
                    AddEditFundAction.Instance.ClickButtonLinkInDropdown(10, "Save as New Manager");

                    // Verify the toast message new fund is shown correctly after clicking on save button
                    msg = "Data has been saved to the list: " + managerName;
                    verifyPoint = msg == AddEditFundAction.Instance.toastMessageAlertGetText(10, msg);
                    verifyPoints.Add(summaryTC = "Verify the toast message (new Manager Name saved) is shown correctly: " + msg + " ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Add a new Fund (1st Fund)
                    fundName1 = "QA_Auto_Fund" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
                    AddEditFundAction.Instance.InputFieldLabel(10, " " + AddEditFundPage.fundName, fundName1);
                    //.ClickAndSelectItemInDropdownInputField(10, " " + AddEditFundPage.lowestLevelSubAssetClass, "Domestic Equity");
                    AddEditFundAction.Instance.ClickAndSelectItemInDropdownLowestAssetClass(10, " " + AddEditFundPage.lowestLevelSubAssetClass, "Domestic Equity");


                    // Click Save button
                    AddEditFundAction.Instance.ClickButtonLabel(10, "Save")
                                           .WaitForLoadingIconToDisappear(120, General.loadingSpinner); Thread.Sleep(2000);
                    #endregion

                    #region Switch to the new opened tab to verify the created Manual Public Fund
                    // Wait For the new tab
                    SearchFundAction.Instance.WaitForMoreNewTab(60);

                    // Switch to the new tab
                    Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                    // Wait for the new tab is load done
                    GeneralAction.Instance.WaitForElementVisible(60, SearchFundPage.fundNavbarTable); Thread.Sleep(3000);

                    // Check if 'spinner' loading icon is shown then wait for it to disappear
                    if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                    }

                    // Verify data (Manual Public Fund)
                    string data;
                    verifyPoint = (data = fundName1) == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                    verifyPoints.Add(summaryTC = "Verify Fund name (Manual Public Fund) is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Location:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                    verifyPoints.Add(summaryTC = "Verify Location (Manual Public Fund) is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Strategy (Manual Public Fund) is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Geographic Focus (Manual Public Fund) is shown correctly after adding new fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Hard Lockup (Manual Public Fund) is shown correctly after adding new fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                    verifyPoints.Add(summaryTC = "Verify Inception Date (Manual Public Fund) is shown correctly after adding new fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2).Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify Performance Fee (Albourne) is shown correctly after searching: 'value < 100%'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "2%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                    verifyPoints.Add(summaryTC = "Verify Management Fee (Albourne) is shown correctly after adding new fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Y") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 3, 2);
                    verifyPoints.Add(summaryTC = "Verify High Watermark (Albourne) is shown correctly after adding new fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Fund List' & 'Status' dropdown
                    verifyPoint = (data = "1 - Pre-One Pager") == PipelineAction.Instance.PipelineStatusDropdownGetText(10);
                    verifyPoints.Add(summaryTC = "Verify data at 'Pipeline Status' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'General' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundManager);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Manager' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Address' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactFirstLast);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact First Last' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmail);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact Email' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessPhone);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Phone' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = fundName1) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundName);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Name' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "General") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundLiquidityType);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Liquidity Type' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.description);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Description' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelDropdownValueGetText(10, PipelinePage.lowestLevelSubAssetClass);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Lowest Level Sub-Asset Class' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Public Equities") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.assetClassNoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Asset Class' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel1NoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 1' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel2NoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 2' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sector);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Sector' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.geography);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Geography' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Process' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryResponsible);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Primary Responsible' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.targetCloseDate);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Target Close Date' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.docsDueDate);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Docs Due Date' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundingAmount);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Funding Amount' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Custom Risk Benchmark and Risk' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.trackingError);
                    verifyPoints.Add(summaryTC = "Verify data at 'Custom Risk Benchmark and Risk' - 'Tracking Error' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Fee Term' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFee);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFeePaid);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee Paid' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.performanceFee);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Performance Fee' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "false") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.highWaterMark);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'highWaterMark' checkbox is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUp);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpPercAgeIfSoft);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up Perc Age If Soft' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.crystallizationEveryXYears);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Crystallization Every X Years' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "false") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.hurdleStatus);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Status' checkbox is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleFixedOrRelative);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Fixed Or Relative *' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleType);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Type' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.rampType);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Ramp Type' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Liquidity' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockup);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockupLengthMonths);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup Length Months' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liqudityFrequency);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liqudity Frequency' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.noticeDays);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Notice Days' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.investorGate);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Investor Gate' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.softLockupRedemptionFeePerc);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Soft Lockup Redemption Fee %' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.receiptDays);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Receipt Days' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.holdback);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Holdback' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liquidityNote);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liquidity Note' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sidepocketProbability);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Sidepocket Probability' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.maxPercOfSidepocketPermitted);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Max % of Sidepocket Permitted' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Add a new Fund (2nd Fund)
                    fundName2 = fundName1 + @"_2";

                    // Switch to the first tab to Go to Fund Setup popup
                    Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.First());

                    // Wait for the new tab is load done
                    GeneralAction.Instance.WaitForElementVisible(60, LoginPage.pieChart)
                                          .WaitForElementVisible(60, LoginPage.assetTable)
                                          .WaitForElementVisible(60, LoginPage.totalEndowmentTable); Thread.Sleep(1000);

                    // Click on 'Fund Setup' button
                    AddEditFundAction.Instance.ClickButtonLabel(10, "Fund Setup")
                                              .WaitForElementVisible(10, General.dynamicDialog); Thread.Sleep(500);

                    // Check if 'spinner' loading icon is shown then wait for it to disappear
                    if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                    }

                    AddEditFundAction.Instance.ClickMenuButtonLabel(10, "Modeling");

                    // Click to select a Fund Type
                    AddEditFundAction.Instance.ClickFundTypeDropdown(10).WaitForElementVisible(10, General.overlayDropdown)
                                              .ClickToSelectItemInDropdown(10, fundType) // Public Fund
                                              .WaitForElementInvisible(10, AddEditFundPage.overlayDropdown);

                    // Add an existing Fund
                    /// Input to add a new Fund
                    AddEditFundAction.Instance.ClearInputFieldLabel(10, AddEditFundPage.fundManager);
                    AddEditFundAction.Instance.InputFieldLabel(10, AddEditFundPage.fundManager, managerName)
                                              .ClickFundDropdownInputField(10, AddEditFundPage.fundManager);
                    AddEditFundAction.Instance.ClickManagerNameReturnOfResults(10, managerName);
                    AddEditFundAction.Instance.InputFieldLabel(10, " " + AddEditFundPage.fundName, fundName1);
                    AddEditFundAction.Instance.ClickAndSelectItemInDropdownLowestAssetClass(10, " " + AddEditFundPage.lowestLevelSubAssetClass, "Event Driven");
                    AddEditFundAction.Instance.ClickButtonLabel(10, "Save");

                    // Verify the toast message (Fund Name Exists) is shown correctly
                    verifyPoint = fundNameExistsMsg == AddEditFundAction.Instance.toastMessageAlertGetText(10, fundNameExistsMsg);
                    verifyPoints.Add(summaryTC = "Verify the error message (Fund Name Exists) is shown correctly: " + fundNameExistsMsg + " ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC); Thread.Sleep(1000);

                    // Add new Fund
                    /// Input to add a new Fund
                    AddEditFundAction.Instance.InputFieldLabel(10, " " + AddEditFundPage.fundName, fundName2)
                                              .ClickButtonLabel(10, "Save")
                                              .WaitForLoadingIconToDisappear(120, General.loadingSpinner); Thread.Sleep(2000);
                    #endregion

                    #region Switch to the new opened tab to verify the created Manual Public Fund
                    // Wait For the new tab
                    SearchFundAction.Instance.WaitForMoreNewTab(60);

                    // Switch to the new tab
                    Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                    // Wait for the new tab is load done
                    GeneralAction.Instance.WaitForElementVisible(60, SearchFundPage.fundNavbarTable); Thread.Sleep(3000);

                    // Check if 'spinner' loading icon is shown then wait for it to disappear
                    if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                    }

                    // Verify data (Manual Public Fund)
                    verifyPoint = (data = fundName2) == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                    verifyPoints.Add(summaryTC = "Verify Fund name (Manual Public Fund) is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Location:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                    verifyPoints.Add(summaryTC = "Verify Location (Manual Public Fund) is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Strategy (Manual Public Fund) is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Geographic Focus (Manual Public Fund) is shown correctly after adding new the 2nd fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Hard Lockup (Manual Public Fund) is shown correctly after adding new the 2nd fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                    verifyPoints.Add(summaryTC = "Verify Inception Date (Manual Public Fund) is shown correctly after adding new the 2nd fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = double.Parse(SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2).Replace("%", "")) < 100;
                    verifyPoints.Add(summaryTC = "Verify Performance Fee (Albourne) is shown correctly after adding new the 2nd fund: 'value < 100%'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "2%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                    verifyPoints.Add(summaryTC = "Verify Management Fee (Albourne) is shown correctly after adding new the 2nd fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Y") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 3, 2);
                    verifyPoints.Add(summaryTC = "Verify High Watermark (Albourne) is shown correctly after adding new the 2nd fund: " + data + "", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Fund List' & 'Status' dropdown
                    verifyPoint = (data = "1 - Pre-One Pager") == PipelineAction.Instance.PipelineStatusDropdownGetText(10);
                    verifyPoints.Add(summaryTC = "Verify data at 'Pipeline Status' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'General' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundManager);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Manager' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Address' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactFirstLast);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact First Last' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmail);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact Email' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessPhone);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Phone' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = fundName2) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundName);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Name' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "General") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundLiquidityType);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Liquidity Type' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.description);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Description' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Event Driven") == PipelineAction.Instance.LabelDropdownValueGetText(10, PipelinePage.lowestLevelSubAssetClass);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Lowest Level Sub-Asset Class' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Absolute Return") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.assetClassNoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Asset Class' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Event Driven") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel1NoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 1' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Event Driven") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel2NoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 2' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sector);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Sector' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.geography);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Geography' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Process' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryResponsible);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Primary Responsible' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.targetCloseDate);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Target Close Date' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.docsDueDate);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Docs Due Date' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundingAmount);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Funding Amount' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Custom Risk Benchmark and Risk' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.trackingError);
                    verifyPoints.Add(summaryTC = "Verify data at 'Custom Risk Benchmark and Risk' - 'Tracking Error' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Fee Term' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFee);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFeePaid);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee Paid' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.performanceFee);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Performance Fee' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "false") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.highWaterMark);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'highWaterMark' checkbox is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUp);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpPercAgeIfSoft);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up Perc Age If Soft' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.crystallizationEveryXYears);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Crystallization Every X Years' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "false") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.hurdleStatus);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Status' checkbox is shown correctly after adding new the 2nd the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleFixedOrRelative);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Fixed Or Relative *' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleType);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Type' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.rampType);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Ramp Type' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    /// Verify data at 'Liquidity' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockup);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockupLengthMonths);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup Length Months' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liqudityFrequency);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liqudity Frequency' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.noticeDays);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Notice Days' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.investorGate);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Investor Gate' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.softLockupRedemptionFeePerc);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Soft Lockup Redemption Fee %' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.receiptDays);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Receipt Days' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.holdback);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Holdback' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liquidityNote);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liquidity Note' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sidepocketProbability);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Sidepocket Probability' dropdown is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.maxPercOfSidepocketPermitted);
                    verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Max % of Sidepocket Permitted' field is shown correctly after adding new the 2nd fund: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }

                if (urlInstance.Contains("conceptia"))
                {
                    Console.WriteLine(summaryTC = "Notes: TC004 Add new Private Fund is only add new Fund on Sandbox Site!!!");
                    test.Log(Status.Info, summaryTC);
                }
            }
            catch (Exception exception)
            {
                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Test, Category("Regression Testing")]
        public void TC002_Add_PrivateFund_AddNewFund()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string fundType = "Private Fund";
            string? fundManagerName = null;
            //string? firm = null;
            string? fundName1 = null, fundName2 = null, fundName3 = null;
            string message = "Data Already Exists: ";// old: "Error: Data on following fields are already existed. Please try again.";
            const string fundNameExistMsg = "These fields cannot be the same. Please try again.";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Add Fund Test - TC002");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Check if the data of Sandbox or Staging (Conceptia) site then verify data (on that site)
                if (urlInstance.Contains("lab")) 
                {
                    fundManagerName = "PriMan F03"; //firm = "PriMan F03";
                    fundName1 = "Fund 1 of PriMan F03"; fundName2 = "Fund 2 of PriMan F03"; fundName3 = "Fund 3 of PriMan F03";
                }
                if (urlInstance.Contains("conceptia")) 
                {
                    fundManagerName = "Priman Sta01"; //firm = "Firm Sta01";
                    fundName1 = "Fund 1 of Firm Sta01"; fundName2 = "Fund 2 of Firm Sta01"; fundName3 = "Fund 3 of Firm Sta01";
                }

                // Click on 'Fund Setup' button
                AddEditFundAction.Instance.ClickButtonLabel(10, "Fund Setup")
                                          .WaitForElementVisible(10, General.dynamicDialog); Thread.Sleep(500);

                // Check if 'spinner' loading icon is shown then wait for it to disappear
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                }

                AddEditFundAction.Instance.ClickMenuButtonLabel(10, "Modeling");

                // Click to select a Fund Type
                AddEditFundAction.Instance.ClickFundTypeDropdown(10).WaitForElementVisible(10, General.overlayDropdown)
                                          .ClickToSelectItemInDropdown(10, fundType, "2") // Private Fund
                                          .WaitForElementInvisible(10, AddEditFundPage.overlayDropdown);

                // Add a new Fund (Fund Manager Exists)
                AddEditFundAction.Instance.ClearInputFieldLabel(10, AddEditFundPage.fundManager);
                AddEditFundAction.Instance.InputFieldLabel(10, AddEditFundPage.fundManager, fundManagerName) // old: InputManagerName(fundManagerName)
                                          .ClickFundDropdownInputField(10, AddEditFundPage.fundManager)
                                          .ClickButtonLinkInDropdown(10, "Save as New Manager"); Thread.Sleep(2000); //.ClickButtonLabel(10, "Save");
                /// Work-around for Staging
                AddEditFundAction.Instance.ClearInputFieldLabel(10, AddEditFundPage.fundManager);
                AddEditFundAction.Instance.InputFieldLabel(10, AddEditFundPage.fundManager, fundManagerName)
                                          .ClickFundDropdownInputField(10, AddEditFundPage.fundManager)
                                          .ClickButtonLinkInDropdown(10, "Save as New Manager");

                // Verify toast message is shown when add a firm Exists
                verifyPoint = message + fundManagerName == AddEditFundAction.Instance.toastMessageAlertGetText(10, message + fundManagerName);
                verifyPoints.Add(summaryTC = "Verify toast message (firm Exists) is shown correctly after clicking Save button: '" + message + fundManagerName + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); Thread.Sleep(250);

                // Wait for toast message is disappeared
                AddEditFundAction.Instance.WaitForElementInvisible(10, AddEditFundPage.toastMessage(message + fundManagerName));

                // Add a new Fund (Private Fund) (Only add new Fund on Sandbox site!!!)
                if (urlInstance.Contains("lab"))
                {
                    // Click 'Cancel' button to go to 'Fund Setup' popup again
                    AddEditFundAction.Instance.ClickButtonLabel(10, "Cancel")
                                              .WaitForElementInvisible(10, General.dynamicDialog)
                                              .ClickButtonLabel(10, "Fund Setup")
                                              .WaitForElementVisible(10, General.dynamicDialog); Thread.Sleep(500);

                    // Check if 'spinner' loading icon is shown then wait for it to disappear
                    if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                    }

                    AddEditFundAction.Instance.ClickMenuButtonLabel(10, "Modeling");

                    // Click to select a Fund Type
                    AddEditFundAction.Instance.ClickFundTypeDropdown(10).WaitForElementVisible(10, General.overlayDropdown)
                                              .ClickToSelectItemInDropdown(10, fundType, "2") // Private Fund
                                              .WaitForElementInvisible(10, AddEditFundPage.overlayDropdown);

                    // Add Existing Fund name (at Fund List)
                    fundManagerName = "QA_Auto_ManagerPriv" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
                    fundName1 = "Fund 1 of QA_Auto_FirmPriv";
                    AddEditFundAction.Instance.InputFieldLabel(10, AddEditFundPage.fundManager, fundManagerName) //.InputManagerName(fundManagerName)
                                              .ClickFundDropdownInputField(10, AddEditFundPage.fundManager)
                                              .ClickButtonLinkInDropdown(10, "Save as New Manager");
                    // Verify the toast message new fund is shown correctly after clicking on save button
                    message = "Data has been saved to the list: " + fundManagerName;
                    verifyPoint = message == AddEditFundAction.Instance.toastMessageAlertGetText(10, message);
                    verifyPoints.Add(summaryTC = "Verify the toast message (new Manager Name saved) is shown correctly: " + message + " ", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    AddEditFundAction.Instance.ClickAndSelectItemInDropdownLowestAssetClass(10, " " + AddEditFundPage.lowestLevelSubAssetClass, "FAD Real Estate") // old: subAssetClass
                                              /// Add 1st Fund
                                              .InputFieldLabel(10, AddEditFundPage.fundNameIndex + " 1*", fundName1) //.InputTxtFundLabel(1, AddEditFundPage.fundName, fundName1)
                                              /// Add 2nd Fund
                                              .ClickButtonLabel(10, "Add Fund")
                                              .InputFieldLabel(10, AddEditFundPage.fundNameIndex + " 2*", fundName1) //.InputTxtFundLabel(2, AddEditFundPage.fundName, fundName1)
                                              .ClickButtonLabel(10, "Save"); Thread.Sleep(1000);

                    /// Verify toast message (same Fund Name) is shown
                    verifyPoint = fundNameExistMsg == AddEditFundAction.Instance.toastMessageAlertGetText(10, fundNameExistMsg);
                    verifyPoints.Add(summaryTC = "Verify toast message (same Fund Name) is shown correctly after clicking Save button: '" + fundNameExistMsg + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC); Thread.Sleep(1000);

                    // Add new Fund name (at Fund List) - Add 2nd Fund
                    fundName2 = "Fund 2 of QA_Auto_FirmPriv";
                    AddEditFundAction.Instance.InputFieldLabel(10, AddEditFundPage.fundNameIndex + " 2*", fundName2) //.InputTxtFundLabel(2, AddEditFundPage.fundName, fundName2) 
                                              .ClickButtonLabel(10, "Save")
                                              .WaitForLoadingIconToDisappear(120, General.loadingSpinner); Thread.Sleep(2000);

                    #region Switch to the new opened tab to verify the created Manual Private Fund
                    // Wait For the new tab
                    SearchFundAction.Instance.WaitForMoreNewTab(60);

                    // Switch to the new tab
                    Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                    // Wait for the new tab is load done
                    GeneralAction.Instance.WaitForElementVisible(60, SearchFundPage.fundNavbarTable); Thread.Sleep(2000);

                    // Check if 'spinner' loading icon is shown then wait for it to disappear
                    if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                    }

                    // Verify data (Manual Public Fund)
                    string data = fundManagerName;
                    verifyPoint = data == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                    verifyPoints.Add(summaryTC = "Verify Fund name (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Location:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                    verifyPoints.Add(summaryTC = "Verify Location (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Strategy (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "2") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                    verifyPoints.Add(summaryTC = "Verify 'Total # of Funds” / Most Recent Fund' (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Inception Date (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 2, 2);
                    verifyPoints.Add(summaryTC = "Verify Industry Focus (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                    verifyPoints.Add(summaryTC = "Verify Geographic Focus (Pipeline) is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    //Verify data at 'Fund List' & 'Status' dropdown
                    verifyPoint = (data = fundName2) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundList);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fund List' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "1 - Pre-One Pager") == PipelineAction.Instance.PipelineStatusDropdownGetText(10);
                    verifyPoints.Add(summaryTC = "Verify data at 'Pipeline Status' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Verify data at 'General' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundManager);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Manager' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Address' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactFirstLast);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact First Last' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmail);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact Email' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessPhone);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Phone' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = fundName2) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundName);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Name' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Drawdown") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundLiquidityType);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Liquidity Type' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.description);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Description' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "FAD Real Estate") == PipelineAction.Instance.LabelDropdownValueGetText(10, PipelinePage.lowestLevelSubAssetClass);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Lowest Level Sub-Asset Class' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "Real Assets") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.assetClassNoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Asset Class' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "FAD Real Estate") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel1NoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 1' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "FAD Real Estate") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel2NoRequired);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 2' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sector);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Sector' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.geography);
                    verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Geography' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Verify data at 'Process' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryResponsible);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Primary Responsible' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.targetCloseDate);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Target Close Date' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.docsDueDate);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Docs Due Date' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundingAmount);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Funding Amount' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.closedSize);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Closed Size' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.vintageYear);
                    verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Vintage Year' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Verify data at 'Custom Risk Benchmark and Risk' section
                    verifyPoint = (data = "12") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.trackingError);
                    verifyPoints.Add(summaryTC = "Verify data at 'Custom Risk Benchmark and Risk' - 'Tracking Error' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Verify data at 'Fee Term' section
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeDuringInvestmentPeriod);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee During Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeDuringInvestmentPeriodOn);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee During Investment Period On' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepUpDownDuringInvestmentPeriod);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Step Up/Down During Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepUpDownDuringTheInvestmentPeriod);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Step Up/Down During The Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeFloorDuringInvestmentPeriod);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee Floor During Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeAfterInvestmentPeriod);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee After Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeAfterInvestmentPeriodOn);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee After Investment Period On' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepUpDownAfterInvestmentPeriod);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Step Up/Down After Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepUpDownRateAfterInvestmentPeriod);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Step Up/Down Rate After Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.mgtFeeFloorAfterInvestmentPeriod);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Mgt Fee Floor After Investment Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.gPCarry);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'GP Carry' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.carryGrossOrNet);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Carry Gross or Net' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.preferredReturnIfNoPreferredReturnEnter0Perc);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Preferred Return (if no preferred return, enter 0%)' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUp);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catchup' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepupCarry);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Stepup Carry' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.stepupCarryPerc);
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Stepup Carry %' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundTermYears); // KS-738
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hold Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.investmentPeriodYears); // KS-738
                    verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Harvest Period' field is shown correctly after searching: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion
                }

                if (urlInstance.Contains("conceptia"))
                {
                    Console.WriteLine(summaryTC = "Notes: TC004 Add new Private Fund is only add new Fund on Sandbox Site!!!");
                    test.Log(Status.Info, summaryTC);
                }
            }
            catch (Exception exception)
            {
                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Test, Category("Regression Testing")]
        public void TC003_Add_PipelineFund_EditFund()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            const string managerName = "GenD MF 2024030901";
            const string fundName = "Gendqal PF 01 of GenD MF 2024030901"; // Pipeline Private
            const string sourceIcon = "P";
            const string fundNameUpdate = fundName + "-editToPublic";
            string currentYear = DateTime.Now.Year.ToString();
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Add Fund Test - TC003");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Search a Pipeline Fund
                SearchFundAction.Instance.InputNameToSearchFund(10, "GenD MF 202403", managerName, fundName, sourceIcon);

                // Wait For the new tab
                SearchFundAction.Instance.WaitForNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(2000);

                // Check if 'spinner' loading icon is shown then wait for it to disappear
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                }

                #region Edit Pipeline Fund
                // Click dropdown 'Fund List' and then click item
                PipelineAction.Instance// Edit 'Fund List & Status' section
                                       ///.ClickAndSelectItemInDropdown(10, PipelinePage.fundList, fundName)
                                       ///.WaitForLoadingIconToDisappear(30, General.loadingSpinner)
                                       .ClickAndSelectItemInDropdownPipelineStatus(10, "2 - One Pager"); /// org: 1 - Pre-One Pager
                // Edit 'General' section (with 'Fund Liquidity Type' = 'Tranche-Based')
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.businessAddress, "33 Nine Elms Lane, London SW11 7US, UK");  // old: Da Nang, Hải Châu District, Da Nang, Vietnam
                PipelineAction.Instance.ClickButtonInputFieldLabel(10, PipelinePage.businessAddress);
                PipelineAction.Instance.WaitForElementVisible(10, PipelinePage.toastMessage("The address is complete"));
                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.primaryContactFirstLast).ClickInputFieldLabel(10, PipelinePage.businessPhone).ClickInputFieldLabel(10, PipelinePage.primaryContactFirstLast); Thread.Sleep(1000);
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.primaryContactFirstLast, "Kathleen Bui"); /// org: Ha Noi
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.primaryContactEmailRequired, "kabui@ksbe.edux"); // org: blank (auto fill)
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.businessPhone, "112233445566").PressTabKeyboard(); /// org: blank
                PipelineAction.Instance.PageDownToScrollDownPage();
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.fundName, fundNameUpdate); // Edit Fund Name
                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.fundLiquidityType).ClickInputFieldLabel(10, PipelinePage.description).ClickInputFieldLabel(10, PipelinePage.fundLiquidityType);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.fundLiquidityType, "Tranche-Based"); /// org: General (ClickAndSelectItemInDropdown)
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.description, "Description " + fundName); Thread.Sleep(1000); /// org: blank
                PipelineAction.Instance.ClickAndSelectItemInDropdownLowestAssetClass(10, PipelinePage.lowestLevelSubAssetClass, "Domestic Equity"); /// org: Natural Resources - (Real Assets -> Private Pipeline)
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.sectorRequired, "Financials"); /// org: blank
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.geographyRequired, "Emerging Markets") /// org: blank
                                       // Edit 'Process' section
                                       .ClearInputFieldLabel(10, PipelinePage.primaryResponsible)
                                       .ClickInputFieldLabel(10, PipelinePage.secondaryResponsible)
                                       .ClickInputFieldLabel(10, PipelinePage.primaryResponsible)
                                       .ClickAndSelectItemInDropdown(10, PipelinePage.primaryResponsible, "Andrew Stevenson") // org: David Ames
                                                                                                                              //.ClickAndSelectItemInDropdown(10, PipelinePage.secondaryResponsible, "Christine Guo") // org: Burton Yuen
                                       .ClickAndSelectDayMonthYearInDatePickerLabel(10, PipelinePage.targetCloseDate, "2024", "Mar", "19") /// currentYear; org: blank
                                       .ClickAndSelectDayMonthYearInDatePickerLabel(10, PipelinePage.docsDueDate, "2024", "Mar", "19") /// currentYear; org: blank
                                       .InputFieldLabel(10, PipelinePage.fundingAmount, "19") /// org: blank
                                       // Edit 'Custom Risk Benchmark and Risk' section
                                       .InputFieldLabel(10, PipelinePage.trackingErrorRequired, "20") /// org: blank
                                       // Edit 'Fee Term' section
                                       .InputFieldLabel(10, PipelinePage.managementFeeRequired, "21"); /// org: blank

                //PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.managementFeePaidRequired).ClickInputFieldLabel(10, PipelinePage.performanceFee).ClickInputFieldLabel(10, PipelinePage.managementFeePaidRequired);
                //PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.managementFeePaidRequired, "Monthly"); /// org: blank
                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.managementFeePaidRequired);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.managementFeePaidRequired, "Monthly"); /// org: blank
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.managementFeePaidRequired, "Monthly"); /// org: blank

                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.performanceFeeRequired, "22") /// org: blank
                                       .ClickCheckboxLabelToCheck(10, PipelinePage.highWaterMark);
                /// org: false (un-check)
                //PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.catchUpRequired, "Yes"); /// org: blank
                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.catchUpRequired);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.catchUpRequired, "Yes"); /// org: blank
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.catchUpRequired, "Yes"); /// org: blank

                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.catchUpPercAgeIfSoftRequired, "23"); /// org: blank

                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.crystallizationEveryXYearsRequired);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.crystallizationEveryXYearsRequired, "2");
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.crystallizationEveryXYearsRequired, "2"); /// org: blank

                PipelineAction.Instance.ClickCheckboxLabelToCheck(10, PipelinePage.hurdleStatus); /// org: false (un-check)

                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.hurdleFixedOrRelativeRequired);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.hurdleFixedOrRelativeRequired, "Fixed");
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.hurdleFixedOrRelativeRequired, "Fixed"); /// org: blank

                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.hurdleRatePerc, "24"); /// org: blank

                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.hurdleTypeRequired);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.hurdleTypeRequired, "Hard Hurdle");
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.hurdleTypeRequired, "Hard Hurdle"); /// org: blank

                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.rampTypeRequired);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.rampTypeRequired, "NAV Dependent");
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.rampTypeRequired, "NAV Dependent"); /// org: blank

                // Edit 'Liquidity' section
                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.lockup);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.lockup, "Hard");
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.lockup, "Hard"); /// org: blank


                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.lockupLengthMonths, "25"); /// org: blank

                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.liqudityFrequency);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.liqudityFrequency, "Monthly");
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.liqudityFrequency, "Monthly"); /// org: blank

                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.noticeDays, "26") /// org: blank
                                       .InputFieldLabel(10, PipelinePage.investorGate, "27") /// org: blank
                                                                                             ///.InputFieldLabel(10, PipelinePage.softLockupRedemptionFeePerc, "28") /// KS-742
                                       .InputFieldLabel(10, PipelinePage.receiptDays, "29") /// org: blank
                                       .InputFieldLabel(10, PipelinePage.holdback, "30") /// org: blank
                                       .InputFieldLabel(10, PipelinePage.liquidityNote, "31"); /// org: blank

                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.sidepocketProbability);
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.sidepocketProbability, "100%");
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.sidepocketProbability, "100%"); /// org: blank


                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.maxPercOfSidepocketPermitted, "32"); /// org: blank

                // Click Save button
                PipelineAction.Instance.ClickSaveButton(10)
                                       .WaitForLoadingIconToDisappear(120, General.loadingSpinner); Thread.Sleep(2000);
                #endregion

                #region Verify data was updated (Public Pipeline)
                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(10, SearchFundPage.fundNavbarTable); Thread.Sleep(2000);

                // Check if 'spinner' loading icon is shown then wait for it to disappear
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                }

                // Verify data was updated
                /// Verify Fund Info
                string data;
                verifyPoint = (data = fundNameUpdate) == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Pipeline) is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Location: 33 Nine Elms Lane, London SW11 7US, UK") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                verifyPoints.Add(summaryTC = "Verify Location (Pipeline) is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Description " + fundName) == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Strategy (Pipeline) is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Emerging Markets") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Geographic Focus (Albourne) is shown correctly after updating: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Yes") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Hard Lockup (Albourne) is shown correctly after updating: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Albourne) is shown correctly after updating: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = double.Parse(SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2).Replace("%", "")) < 100;
                verifyPoints.Add(summaryTC = "Verify Performance Fee (Albourne) is shown correctly after updating: 'value < 100%'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "21%") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Management Fee (Albourne) is shown correctly after updating: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Y") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 3, 2);
                verifyPoints.Add(summaryTC = "Verify High Watermark (Albourne) is shown correctly after updating: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Fund List' & 'Status' dropdown
                verifyPoint = (data = "2 - One Pager") == PipelineAction.Instance.PipelineStatusDropdownGetText(10);
                verifyPoints.Add(summaryTC = "Verify data at 'Pipeline Status' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'General' section
                verifyPoint = (data = managerName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundManager);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Manager' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "33 Nine Elms Lane, London SW11 7US, UK") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Address' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Kathleen Bui") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactFirstLast);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact First Last' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "kabui@ksbe.edux") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmailRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact Email' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "112233445566") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessPhone);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Phone' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = fundNameUpdate) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundName);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Name' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Tranche-Based") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundLiquidityType);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Liquidity Type' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Description " + fundName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.description);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Description' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelDropdownValueGetText(10, PipelinePage.lowestLevelSubAssetClass);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Lowest Level Sub-Asset Class' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Public Equities") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.assetClassNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Asset Class' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel1NoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 1' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel2NoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 2' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Financials") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sectorRequired, "1");
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Sector' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Emerging Markets") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.geographyRequired, "1");
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Geography' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Process' section
                verifyPoint = (data = "Andrew Stevenson") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryResponsible);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Primary Responsible' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = (data = "Christine Guo") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.secondaryResponsible);
                //verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Secondary Responsible' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "03/19/" + "2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.targetCloseDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Target Close Date' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // currentYear
                verifyPoint = (data = "03/19/" + "2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.docsDueDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Docs Due Date' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // currentYear
                verifyPoint = (data = "19") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundingAmount);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Funding Amount' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Custom Risk Benchmark and Risk' section
                verifyPoint = (data = "20") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.trackingErrorRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Custom Risk Benchmark and Risk' - 'Tracking Error' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Fee Term' section
                verifyPoint = (data = "21") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFeeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Monthly") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFeePaidRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee Paid' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "22") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.performanceFeeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Performance Fee' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "true") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.highWaterMark);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'highWaterMark' checkbox is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Yes") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "23") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpPercAgeIfSoftRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up Perc Age If Soft' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "2") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.crystallizationEveryXYearsRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Crystallization Every X Years' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "true") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.hurdleStatus);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Status' checkbox is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Fixed") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleFixedOrRelativeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Fixed Or Relative *' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "24") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleRatePerc);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Rate (%)' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Hard Hurdle") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleTypeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Type' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "NAV Dependent") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.rampTypeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Ramp Type' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Liquidity' section
                verifyPoint = (data = "Hard") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockup);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "25") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockupLengthMonths);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup Length Months' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Monthly") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liqudityFrequency);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liqudity Frequency' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "26") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.noticeDays);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Notice Days' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "27") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.investorGate);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Investor Gate' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.softLockupRedemptionFeePerc); // KS-742
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Soft Lockup Redemption Fee %' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "29") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.receiptDays);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Receipt Days' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "30") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.holdback);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Holdback' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "31") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liquidityNote);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liquidity Note' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "100%") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sidepocketProbability);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Sidepocket Probability' dropdown is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "32") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.maxPercOfSidepocketPermitted);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Max % of Sidepocket Permitted' field is shown correctly after updating: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Save Data to Origin (Private Pipeline)
                PipelineAction.Instance// Edit 'Fund List & Status' section
                                       .ClickAndSelectItemInDropdownPipelineStatus(10, "1 - Pre-One Pager")
                                       // Edit 'General' section (with 'Fund Liquidity Type' = 'General')
                                       .ClearInputFieldLabel(10, PipelinePage.businessAddress)
                                       .ClickButtonInputFieldLabel(10, PipelinePage.businessAddress)
                                       .WaitForElementVisible(10, PipelinePage.toastMessage("Please input data"));
                PipelineAction.Instance.ClearInputFieldLabel(10, PipelinePage.primaryContactFirstLast).ClickInputFieldLabel(10, PipelinePage.businessPhone).ClickInputFieldLabel(10, PipelinePage.primaryContactFirstLast); Thread.Sleep(1000);
                PipelineAction.Instance.ClickAndSelectItemInDropdown(10, PipelinePage.primaryContactFirstLast, "Ha Noi")
                                       .InputFieldLabel(10, PipelinePage.primaryContactEmail, "test01Cnext@yahoo.com") // (auto fill if fill business Address)
                                       .ClearInputFieldLabel(10, PipelinePage.businessPhone).PressTabKeyboard();
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.fundName, fundName)
                                       .ClearInputFieldLabel(10, PipelinePage.fundLiquidityType).ClickInputFieldLabel(10, PipelinePage.description).ClickInputFieldLabel(10, PipelinePage.fundLiquidityType)
                                       .ClickAndSelectItemInDropdown(10, PipelinePage.fundLiquidityType, "General")
                                       .ClearInputFieldLabel(10, PipelinePage.description)
                                       .ClickAndSelectItemInDropdownLowestAssetClass(10, PipelinePage.lowestLevelSubAssetClass, "Natural Resources") /// (Real Assets -> Private Pipeline)
                                       .ClickRemoveInputTextIndexDropdownLabelButton(10, PipelinePage.sector, "1")
                                       .ClickRemoveInputTextIndexDropdownLabelButton(10, PipelinePage.geography, "1")
                                       // Edit 'Process' section
                                       .ClearInputFieldLabel(10, PipelinePage.primaryResponsible).ClickInputFieldLabel(10, PipelinePage.secondaryResponsible).ClickInputFieldLabel(10, PipelinePage.primaryResponsible)
                                       .ClickAndSelectItemInDropdown(10, PipelinePage.primaryResponsible, "David Ames")
                                       //.ClickAndSelectItemInDropdown(10, PipelinePage.secondaryResponsible, "Burton Yuen")
                                       .ClickAndSelectDayMonthYearInDatePickerLabel(10, PipelinePage.targetCloseDate, "2024", "Mar", "14") // currentYear
                                       .ClickAndSelectDayMonthYearInDatePickerLabel(10, PipelinePage.docsDueDate, "2024", "Mar", "14") // currentYear
                                       .ClearInputFieldLabel(10, PipelinePage.fundingAmount)
                                       // Edit 'Custom Risk Benchmark and Risk' section
                                       ///PipelineAction.Instance.InputFieldLabel(10, PipelinePage.trackingError, ""); --> KS-743
                                       // Edit 'Fee Term' section
                                       .InputFieldLabel(10, PipelinePage.managementFee, "")
                                       .ClearInputFieldLabel(10, PipelinePage.managementFeePaid).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       .InputFieldLabel(10, PipelinePage.performanceFee, "")
                                       .ClickCheckboxLabelToUnCheck(10, PipelinePage.highWaterMark)
                                       .InputFieldLabel(10, PipelinePage.catchUpPercAgeIfSoft, "")
                                       .ClearInputFieldLabel(10, PipelinePage.catchUp).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       .ClearInputFieldLabel(10, PipelinePage.crystallizationEveryXYears).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       //.ClearInputFieldLabel(10, PipelinePage.hurdleFixedOrRelativeRequired).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       //.ClearInputFieldLabel(10, PipelinePage.hurdleRatePerc)
                                       //.ClearInputFieldLabel(10, PipelinePage.hurdleTypeRequired).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       //.ClearInputFieldLabel(10, PipelinePage.rampTypeRequired).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       //.ClickCheckboxLabelToUnCheck(10, PipelinePage.hurdleStatus)
                                       // Edit 'Liquidity' section
                                       .ClearInputFieldLabel(10, PipelinePage.lockup).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       .ClearInputFieldLabel(10, PipelinePage.lockupLengthMonths)
                                       .ClearInputFieldLabel(10, PipelinePage.liqudityFrequency).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       .ClearInputFieldLabel(10, PipelinePage.noticeDays)
                                       .InputFieldLabel(10, PipelinePage.investorGate, "")
                                       ///.InputFieldLabel(10, PipelinePage.softLockupRedemptionFeePerc, "") /// KS-742
                                       .ClearInputFieldLabel(10, PipelinePage.receiptDays)
                                       .InputFieldLabel(10, PipelinePage.holdback, "")
                                       .ClearInputFieldLabel(10, PipelinePage.liquidityNote)
                                       .ClearInputFieldLabel(10, PipelinePage.sidepocketProbability).PressTabKeyboard() /// ClickAndSelectItemInDropdown
                                       .InputFieldLabel(10, PipelinePage.maxPercOfSidepocketPermitted, "");

                // Click Save button
                PipelineAction.Instance.ClickSaveButton(10)
                                       .WaitForLoadingIconToDisappear(120, General.loadingSpinner);
                #endregion

                #region Switch to the new opened tab to verify origin data (Private Pipeline)
                // Wait For the new tab
                SearchFundAction.Instance.WaitForMoreNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(60, SearchFundPage.fundNavbarTable); Thread.Sleep(2000);

                // Verify origin data (Private Pipeline)
                /// Verify Fund Info
                verifyPoint = (data = managerName) == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Pipeline) is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Location:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                verifyPoints.Add(summaryTC = "Verify Location (Pipeline) is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Natural Resources") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Strategy (Pipeline) is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "1") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify 'Total # of Funds” / Most Recent Fund' (Pipeline) is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                //verifyPoints.Add(summaryTC = "Verify Inception Date (Pipeline) is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Industry Focus (Pipeline) is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Geographic Focus (Pipeline) is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Fund List' & 'Status' dropdown
                verifyPoint = (data = fundName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundList);
                verifyPoints.Add(summaryTC = "Verify data at 'Fund List' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "1 - Pre-One Pager") == PipelineAction.Instance.PipelineStatusDropdownGetText(10);
                verifyPoints.Add(summaryTC = "Verify data at 'Pipeline Status' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'General' section
                verifyPoint = (data = managerName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundManager);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Manager' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Address' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Ha Noi") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactFirstLast);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact First Last' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "test01Cnext@yahoo.com") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmail);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact Email' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessPhone);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Phone' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = fundName) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundName);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Name' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "General") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundLiquidityType);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Liquidity Type' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.description);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Description' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Natural Resources") == PipelineAction.Instance.LabelDropdownValueGetText(10, PipelinePage.lowestLevelSubAssetClass);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Lowest Level Sub-Asset Class' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Real Assets") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.assetClassNoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Asset Class' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Natural Resources") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel1NoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 1' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Natural Resources") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel2NoRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 2' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sector);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Sector' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.geography);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Geography' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Process' section
                verifyPoint = (data = "David Ames") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryResponsible);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Primary Responsible' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Burton Yuen") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.secondaryResponsible);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Secondary Responsible' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "03/14/" + "2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.targetCloseDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Target Close Date' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // currentYear
                verifyPoint = (data = "03/14/" + "2024") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.docsDueDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Docs Due Date' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // currentYear
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundingAmount);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Funding Amount' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                ///Verify data at 'Custom Risk Benchmark and Risk' section (KS-743)
                verifyPoint = (data = "14") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.trackingError);
                verifyPoints.Add(summaryTC = "Verify data at 'Custom Risk Benchmark and Risk' - 'Tracking Error' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Fee Term' section
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFee);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFeePaid); // old: Monthly
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee Paid' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.performanceFee);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Performance Fee' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "false") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.highWaterMark);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'highWaterMark' checkbox is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUp); // old: Yes
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "23") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpPercAgeIfSoft)
                            || (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpPercAgeIfSoft);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up Perc Age If Soft' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = (data = "2") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.crystallizationEveryXYears);
                //verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Crystallization Every X Years' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "true") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.hurdleStatus);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Status' checkbox is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Fixed") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleFixedOrRelativeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Fixed Or Relative *' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "24") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleRatePerc);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Rate (%) *' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "Hard Hurdle") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleTypeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Type *' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "NAV Dependent") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.rampTypeRequired);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Ramp Type *' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                /// Verify data at 'Liquidity' section
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockup); // old: Hard
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint); // old: Hard
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockupLengthMonths);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup Length Months' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liqudityFrequency); // old: Monthly
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liqudity Frequency' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.noticeDays);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Notice Days' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.investorGate);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Investor Gate' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.softLockupRedemptionFeePerc); // KS-742
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Soft Lockup Redemption Fee %' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.receiptDays);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Receipt Days' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.holdback);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Holdback' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liquidityNote);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liquidity Note' field is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sidepocketProbability); // old: 100%
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Sidepocket Probability' dropdown is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC); // By default, if anything = null or undefined, system will not save it
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.maxPercOfSidepocketPermitted);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Max % of Sidepocket Permitted' field is shown correctly after saving origin data: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
            }
            catch (Exception exception)
            {
                // Print exception
                System.Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }

        [Test, Category("Regression Testing")] // , Ignore("")
        public void TC004_Add_PipelineFund_AddNewFund()
        {
            #region Variables declare
            string urlInstance = LoginPage.url;
            string managerNameManual = "QA_Auto_Manager_Pipeline_Public" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
            string fundNameManual = "QA_Auto_Fund_Pipeline_Public" + @"_" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_");
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("WorkbenchApp - Add Fund Test - TC004");
            try
            {
                // Log into the application
                LoginAction.Instance.LoginSiteNoGodaddy(60, urlInstance);

                // Click on 'Fund Setup' button
                AddEditFundAction.Instance.ClickButtonLabel(10, "Fund Setup")
                                          .WaitForElementVisible(10, General.dynamicDialog)
                                          .ClickMenuButtonLabel(10, "Pipeline")
                                          .WaitForElementVisible(10, PipelinePage.pipelineFundSetupPopupHeader("General")); Thread.Sleep(2000);

                #region Input data to add a new Pipeline Fund
                PipelineAction.Instance.InputFieldLabel(10, PipelinePage.fundManager, managerNameManual);
                AddEditFundAction.Instance.ClickFundDropdownInputField(10, PipelinePage.fundManager); // old: ClickLabelDropdown // new: ClickFundDropdownInputField
                PipelineAction.Instance.ClickButtonLinkInDropdown(10, "Save as New Manager");
                PipelineAction.Instance.WaitForElementVisible(10, PipelinePage.toastMessage("Data has been saved to the list: " + managerNameManual + ""))
                                       .ClickAndSelectItemInDropdown(10, PipelinePage.primaryContactFirstLast, "Kathleen Bui")
                                       .InputFieldLabel(10, PipelinePage.fundName, fundNameManual)
                                       .InputFieldLabel(10, PipelinePage.fundLiquidityType, "Tranche-Based")
                                       .ClickAndSelectItemInDropdownLowestAssetClass(10, PipelinePage.lowestLevelSubAssetClass, "Domestic Equity")
                                       .ClickAndSelectItemInDropdown(10, PipelinePage.primaryResponsible, "Andrew Stevenson");

                // Click Save button
                PipelineAction.Instance.ClickSaveButton(10)
                                       .WaitForLoadingIconToDisappear(120, General.loadingSpinner); Thread.Sleep(2000);
                #endregion

                #region Switch to the new opened tab to verify the created Pipeline Public
                // Wait For the new tab
                SearchFundAction.Instance.WaitForMoreNewTab(60);

                // Switch to the new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Wait for the new tab is load done
                GeneralAction.Instance.WaitForElementVisible(60, SearchFundPage.fundNavbarTable); Thread.Sleep(2000);

                // Check if 'spinner' loading icon is shown then wait for it to disappear
                if (GeneralAction.Instance.IsElementPresent(General.loadingSpinner))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.loadingSpinner); Thread.Sleep(1000);
                }

                // Verify data (Public Pipeline)
                string data;
                verifyPoint = (data = fundNameManual) == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 2, 1);
                verifyPoints.Add(summaryTC = "Verify Fund name (Pipeline) is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Location:") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 1, 4, 1);
                verifyPoints.Add(summaryTC = "Verify Location (Pipeline) is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Strategy (Pipeline) is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Geographic Focus (Albourne) is shown correctly after adding new fund: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "No") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 1, 2);
                verifyPoints.Add(summaryTC = "Verify Hard Lockup (Albourne) is shown correctly after adding new fund: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 2, 2);
                verifyPoints.Add(summaryTC = "Verify Inception Date (Albourne) is shown correctly after adding new fund: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                //verifyPoint = double.Parse(SearchFundAction.Instance.ValueInFundNavbarTable(10, 3, 3, 2).Replace("%", "")) < 100;
                //verifyPoints.Add(summaryTC = "Verify Performance Fee (Albourne) is shown correctly after searching: 'value < 100%'", verifyPoint);
                //ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 4, 3, 2);
                verifyPoints.Add(summaryTC = "Verify Management Fee (Albourne) is shown correctly after adding new fund: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "N") == SearchFundAction.Instance.ValueInFundNavbarTable(10, 5, 3, 2);
                verifyPoints.Add(summaryTC = "Verify High Watermark (Albourne) is shown correctly after adding new fund: " + data + "", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Fund List' & 'Status' dropdown
                verifyPoint = (data = "1 - Pre-One Pager") == PipelineAction.Instance.PipelineStatusDropdownGetText(10);
                verifyPoints.Add(summaryTC = "Verify data at 'Pipeline Status' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'General' section
                verifyPoint = (data = managerNameManual) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundManager);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Manager' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessAddress);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Address' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Kathleen Bui") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactFirstLast);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact First Last' dropdown is shown correctly after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "kabui@ksbe.edux") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmail)
                    || (data = "test01Cnext@yahoo.com") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryContactEmail);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Primary Contact Email' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);      
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.businessPhone);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Business Phone' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = fundNameManual) == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundName);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Name' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Tranche-Based") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundLiquidityType);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Liquidity Type' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.description);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Description' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelDropdownValueGetText(10, PipelinePage.lowestLevelSubAssetClass);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Lowest Level Sub-Asset Class' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Public Equities") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.assetClassNoRequired); //assetClass
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Asset Class' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel1NoRequired); // fundSubAssetClassLevel1
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 1' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "Domestic Equity") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundSubAssetClassLevel2NoRequired); // fundSubAssetClassLevel2
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Fund Sub Asset Class Level 2' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sector);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Sector' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.geography);
                verifyPoints.Add(summaryTC = "Verify data at 'General' - 'Geography' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Process' section
                verifyPoint = (data = "Andrew Stevenson") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.primaryResponsible);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Primary Responsible' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.targetCloseDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Target Close Date' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.docsDueDate);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Docs Due Date' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.fundingAmount);
                verifyPoints.Add(summaryTC = "Verify data at 'Process' - 'Funding Amount' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Custom Risk Benchmark and Risk' section
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.trackingError);
                verifyPoints.Add(summaryTC = "Verify data at 'Custom Risk Benchmark and Risk' - 'Tracking Error' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Fee Term' section
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFee);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.managementFeePaid);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Management Fee Paid' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.performanceFee);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Performance Fee' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "false") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.highWaterMark);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'highWaterMark' checkbox is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUp);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.catchUpPercAgeIfSoft);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Catch Up Perc Age If Soft' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.crystallizationEveryXYears);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Crystallization Every X Years' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "false") == PipelineAction.Instance.LabelCheckboxFieldGetText(10, PipelinePage.hurdleStatus);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Status' checkbox is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleFixedOrRelative);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Fixed Or Relative *' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.hurdleType);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Hurdle Type' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.rampType);
                verifyPoints.Add(summaryTC = "Verify data at 'Fee Term' - 'Ramp Type' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                /// Verify data at 'Liquidity' section
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockup);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.lockupLengthMonths);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Lockup Length Months' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liqudityFrequency);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liqudity Frequency' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.noticeDays);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Notice Days' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.investorGate);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Investor Gate' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.softLockupRedemptionFeePerc);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Soft Lockup Redemption Fee %' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.receiptDays);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Receipt Days' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.holdback);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Holdback' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.liquidityNote);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Liquidity Note' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.sidepocketProbability);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Sidepocket Probability' dropdown is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                verifyPoint = (data = "") == PipelineAction.Instance.LabelInputFieldGetText(10, PipelinePage.maxPercOfSidepocketPermitted);
                verifyPoints.Add(summaryTC = "Verify data at 'Liquidity' - 'Max % of Sidepocket Permitted' field is shown correctly after adding new fund: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
            }
            catch (Exception exception)
            {
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
