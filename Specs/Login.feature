Feature: Login
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Succesfull Login
	Given I have entered my correct emailadress
	And I have entered my correct password
	When I press Login
	Then I should be forwarded to the homepage being logged in

Scenario:  login failed
	Given I have entered my correct emailadress
	And I have entered a wrong password
	When I press Login
	Then there should be displayed a message that I have entered a wrong password

Scenario: login failed
	Given I have entered a wrong emailadress
	And I have entered my correct password
	When I press Login
	Then there should be a message that the login failed

