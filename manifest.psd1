#
# Module manifest for module 'manifest'
#
# Generated by: Jan Hoek
#
# Generated on: 20/03/2019
#

@{

    # Script module or binary module file associated with this manifest.
    RootModule        = 'ATDD.TestScriptor.dll'

    # Version number of this module.
    ModuleVersion     = '0.1.3'

    # Supported PSEditions
    # CompatiblePSEditions = @()

    # ID used to uniquely identify this module
    GUID              = '7cef471c-3b16-4620-8b74-5a86c520d989'

    # Author of this module
    Author            = 'Jan Hoek/Luc van Vugt/Peter Conijn'

    # Company or vendor of this module
    CompanyName       = 'fluxxus.nl'

    # Copyright statement for this module
    Copyright         = 'Copyright (c) 2024 fluxxus.nl'

    # Description of the functionality provided by this module
    Description       = 'Acceptance Test-Driven Development test scriptor'

    # Minimum version of the PowerShell engine required by this module
    # PowerShellVersion = ''

    # Name of the PowerShell host required by this module
    # PowerShellHostName = ''

    # Minimum version of the PowerShell host required by this module
    # PowerShellHostVersion = ''

    # Minimum version of Microsoft .NET Framework required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
    # DotNetFrameworkVersion = ''

    # Minimum version of the common language runtime (CLR) required by this module. This prerequisite is valid for the PowerShell Desktop edition only.
    # CLRVersion = ''

    # Processor architecture (None, X86, Amd64) required by this module
    # ProcessorArchitecture = ''

    # Modules that must be imported into the global environment prior to importing this module
    # RequiredModules = @()

    # Assemblies that must be loaded prior to importing this module
    # RequiredAssemblies = @()

    # Script files (.ps1) that are run in the caller's environment prior to importing this module.
    # ScriptsToProcess = @()

    # Type files (.ps1xml) to be loaded when importing this module
    # TypesToProcess = @()

    # Format files (.ps1xml) to be loaded when importing this module
    FormatsToProcess  = 'ATDD.TestScriptor.format.ps1xml'

    # Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
    # NestedModules = @()

    # Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
    FunctionsToExport = '*'

    # Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
    CmdletsToExport   = 'New-ATDDGiven', 'New-ATDDTestFeature', 'New-ATDDTestScenario', 'New-ATDDThen',
    'New-ATDDWhen', 'ConvertTo-ALTestCodeunit', 'New-ATDDCleanup', 'Sync-ALTestCodeunit'

    # Variables to export from this module
    VariablesToExport = '*'

    # Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
    AliasesToExport   = 'Cleanup', 'Given', 'Feature', 'Scenario', 'Then', 'When'

    # DSC resources to export from this module
    # DscResourcesToExport = @()

    # List of all modules packaged with this module
    # ModuleList = @()

    # List of all files packaged with this module
    # FileList = @()

    # Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
    PrivateData       = @{

        PSData = @{

            # Tags applied to this module. These help with module discovery in online galleries.
            # Tags = @()

            # A URL to the license for this module.
            LicenseUri = 'https://github.com/fluxxus-nl/ATDD.TestScriptor/blob/master/LICENSE'

            # A URL to the main website for this project.
            ProjectUri = 'https://github.com/fluxxus-nl/ATDD.TestScriptor'

            # A URL to an icon representing this module.
            IconUri    = 'https://github.com/fluxxus-nl.png'

            # ReleaseNotes of this module
            # ReleaseNotes = ''

        } # End of PSData hashtable

    } # End of PrivateData hashtable

    # HelpInfo URI of this module
    # HelpInfoURI = ''

    # Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
    # DefaultCommandPrefix = ''

}

