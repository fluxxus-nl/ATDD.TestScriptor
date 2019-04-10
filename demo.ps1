# FIXME: Install-Module ATDD.TestScriptor -Force
Import-Module ./output/ATDD.TestScriptor/ATDD.TestScriptor.psd1 -Force

$Features = @()

$Features +=
Feature 'LookupValue UT Customer' {
    Scenario 1 'Check that label can be assigned to customer' {
        Given	'A label'
        Given	'A customer'
        When	'Assign label to customer'
        Then	'Customer has label field populated'
    }

    Scenario 2 'Check that label field table relation is validated for non-existing label on customer' {
        Given	'A non-existing label value'
        Given	'A customer record variable'
        When	'Assign non-existing label to customer'
        Then	'Non existing label error was thrown'
    }

    Scenario 3 'Check that label can be assigned on customer card' {
        Given	'A label'
        Given	'A customer card'
        When	'Assign label to customer card'
        Then	'Customer has label field populated'
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

$Features | `
    ConvertTo-ALTestCodeunit `
    -CodeunitID 81000 `
    -CodeunitName 'LookupValue UT Customer' `
    -InitializeFunction `
    -GivenFunctionName "Make {0}" `
    -WhenFunctionName 'Blaat {0}' `
    -ThenFunctionName "Test {0}"
