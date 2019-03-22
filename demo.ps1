Import-Module -Name "$PSScriptRoot/bin/Debug/netstandard2.0/ATDD.TestScriptor.psd1" -Force

$Features = @()

$Features +=
Feature 'My Feature' {
    Scenario 1 'My First Scenario' {
        Given 'First Given'
        Given 'Second Given'
        When 'This happens'
        Then 'This should happen'
        Then 'This should also happen'
    }

    Scenario 2 'My Second Scenario' {
        Given 'First Given'
        When 'Something happens'
        Then 'Something else should happen'
        Cleanup 'Delete all records'
    }
}

$Features +=
Feature 'Another Feature' {
    Scenario 1 'Oink' {
        Given Boink
        When Kloink
        Then Zoink
    }
}

$Features | ConvertTo-ALTestCodeunit 81000 'LookupValue UT Customer'