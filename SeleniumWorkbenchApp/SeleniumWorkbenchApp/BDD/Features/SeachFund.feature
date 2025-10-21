Feature: Search Fund Functionality
  In order to find Fund quickly
  As a logged-in user
  I want to search for specific fund on the Total Endowment dashboard

  @smoke
  Scenario: Search for the Albourne fund
    Given I am logged in and on the Total Endowment page
    When I search for the Albourne fund
    Then I should see the Albourne fund in the new tab

  #@regression @dashboard
  #Scenario: Search for an invalid keyword
    #Given I am logged in and on the Total Endowment page
    #When I search for the keyword "xyz_not_found"
    #Then I should see a message "No results found"
