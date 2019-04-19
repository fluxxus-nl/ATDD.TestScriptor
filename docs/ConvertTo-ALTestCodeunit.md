---
external help file: ATDD.TestScriptor.dll-Help.xml
Module Name: ATDD.TestScriptor
online version:
schema: 2.0.0
---

# ConvertTo-ALTestCodeunit

## SYNOPSIS
Converts one or more test features to an AL codeunit.

## SYNTAX

```
ConvertTo-ALTestCodeunit [-CodeunitID] <Int32> [-CodeunitName] <String> [-InitializeFunction]
 [-Feature <TestFeature[]>] [-GivenFunctionName <String>] [-WhenFunctionName <String>]
 [-ThenFunctionName <String>] [<CommonParameters>]
```

## DESCRIPTION
{{Fill in the Description}}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -CodeunitID
{{Fill CodeunitID Description}}

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CodeunitName
{{Fill CodeunitName Description}}

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Feature
{{Fill Feature Description}}

```yaml
Type: TestFeature[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -GivenFunctionName
Specify the format for the AL function that is created for a Given element. Use the placeholder {0} to specify where you want the Given's situation description to go. Leaving a space between the placeholder and the rest of your text ensures that it's seen as a separate word, and therefore gets an initial capital letter when converting to title case, e.g. 'Create {0}' for a Given whose situation is 'a Customer' will lead to 'CreateACustomer' as the function name.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InitializeFunction
{{Fill InitializeFunction Description}}

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ThenFunctionName
Specify the format for the AL function that is created for a Then element. Use the placeholder {0} to specify where you want the Then's expected result description to go. Leaving a space between the placeholder and the rest of your text ensures that it's seen as a separate word, and therefore gets an initial capital letter when converting to title case, e.g. 'Verify {0}' for a Given whose situation is 'customer exists' will lead to 'VerifyCustomerExists' as the function name.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -WhenFunctionName
Specify the format for the AL function that is created for a When element. Use the placeholder {0} to specify where you want the When's condition description to go. Leaving a space between the placeholder and the rest of your text ensures that it's seen as a separate word, and therefore gets an initial capital letter when converting to title case.
```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### ATDD.TestScriptor.TestFeature[]
## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
