# ATDD.TestScriptor
The Acceptance Test-Driven Development test scriptor allows the user to define in a managed matter ATDD test scenarios and convert it into a code structure to facilate fast test code development. At this moment this conversion is only implemented for .al

The ATDD pattern is defined by so called tags:

*	FEATURE: defines what feature(s) the test or collection of test cases is testing
*	SCENARIO: defines for a single test the scenario being teste
*	GIVEN: defines what data setup is needed; a test case can have multiple GIVEN tags when data setup is more complex
*	WHEN: defines the action under test; each test case should have only one WHEN tag
*	THEN: defines the result of the action, or more specifically the verification of the result; if multiple results apply, multiple THEN tags will be needed

## Installation instructions
Type either of the following in a PowerShell prompt:

- to install for all users; requires prompt with admin privileges: 
```powershell
Install-Module ATDD.TestScriptor 
```
- to install for current user only; no admin privileges required:
```powershell
Install-Module ATDD.TestScriptor -Scope CurrentUser 
```

Note that you may be asked for confirmation if you didn't previously mark the PowerShell Gallery repository as a trusted module source.

## Build/usage instructions (beta phase)

- Clone this repository to a folder on your machine;
- Only on Windows: open the .csproj file and replace the `cp` command with `copy` in the `<Exec>` elements;
- Open a PowerShell prompt and navigate to your local repository folder;
- Type `dotnet build` and press Enter;
- Type `./demo.ps1` to view sample output.

## Demo

![ATDD.TestScriptor Demo](demo/ATDD.TestScriptor_Demo.gif)
