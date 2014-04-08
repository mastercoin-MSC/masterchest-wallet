' Tag to determine if object can be localized or not    
Enum LocaleTag
    None                ' Not localizable (same as Nothing)
    Text                ' Localized as text
    Numeric             ' Localized as numeric value
    NetworkDependent
End Enum

Enum NetworkStatus
    Synchronizing
    Processing
    Synchronized
End Enum

