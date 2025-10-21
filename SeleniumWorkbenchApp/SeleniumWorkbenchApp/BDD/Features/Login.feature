Feature: Login Functionality
  In order to access the system
  As a valid user
  I want to log in using my credentials and view the dashboard

  @smoke
  Scenario: Successful login with valid credentials
    Given I open the login page
    When I login with credentials "valid"
    Then I should see the dashboard page
