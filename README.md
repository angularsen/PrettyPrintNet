PrettyPrint.NET
==============

Human friendly, textual representations of TimeSpan and file size.

Install
=======
To install PrettyPrint.NET, run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console) or go to the [NuGet site](https://www.nuget.org/packages/PrettyPrintNet/) for the complete relase history.

![Install-Package PrettyPrintNet](Docs/Images/install_package_prettyprintnet.png "Install-Package PrettyPrintNet")

Build Targets:
* .NET 3.5 Client
* Portable .NET 4.0 Profile 328 (Silverlight 5, Win8, WinPhone8.1, WinPhoneSl8, Monotouch, Monoandroid)
* Portable .NET 4.5 Profile 259 (Win8, WinPhone8.1, WinPhoneSl8, Monotouch, Monoandroid)

Features
========
## TimeSpan.ToPrettyString()
```csharp
var t = new TimeSpan(hours: 3, minutes: 4, seconds: 0);

// Default is 1 unit, long representation, use units from days to seconds, round smallest unit down
t.ToPrettyString();                                             // "3 hours"

// 3 units requested, but seconds is zero and skipped
t.ToPrettyString(3);                                            // "3 hours and 4 minutes"

// Four different unit representations
t.ToPrettyString(2, UnitStringRepresentation.Long);             // "3 hours and 4 minutes"
t.ToPrettyString(2, UnitStringRepresentation.Short);            // "3 hrs 4 mins"
t.ToPrettyString(2, UnitStringRepresentation.CompactWithSpace); // "3h 4m"
t.ToPrettyString(2, UnitStringRepresentation.Compact);          // "3h4m"

// Three types of rounding of the smallest unit, defaulting to 'ToNearestOrUp'
// As an example, ToTimeRemainingString() uses IntegerRounding.Up to not
// show "0 seconds" remaining when there is 0.9 seconds remaining.
var t2 = new TimeSpan(hours: 3, minutes: 30, seconds: 0);
t2.ToPrettyString(1, lowestUnitRounding: IntegerRounding.Down);          // "3 hours"
t2.ToPrettyString(1, lowestUnitRounding: IntegerRounding.Up);            // "4 hours"
t2.ToPrettyString(1, lowestUnitRounding: IntegerRounding.ToNearestOrUp); // "4 hours"
```

## TimeSpan.ToTimeRemainingString()
This is helpful to avoid showing strings like "0 seconds remaining" or "9 seconds remaining" when it really is 9.999 seconds remaining. It basically just calls ```ToPrettyString()``` with ```IntegerRounding.Up```.
```csharp
TimeSpan.FromSeconds(  60.1).TotimeRemainingString(); // "1 minute and 1 second"
TimeSpan.FromSeconds(  60  ).TotimeRemainingString(); // "1 minute"
TimeSpan.FromSeconds(  59.9).TotimeRemainingString(); // "1 minute"
TimeSpan.FromSeconds(   1.1).TotimeRemainingString(); // "2 seconds"
TimeSpan.FromSeconds(   1  ).TotimeRemainingString(); // "1 second"
TimeSpan.FromSeconds(   0.1).TotimeRemainingString(); // "1 second"
TimeSpan.FromSeconds(   0  ).TotimeRemainingString(); // "0 seconds" 
```

## TODO Document FileSizeExtensions
 
Other things to add
===========
* File transfer rate
* Money
* More cultures and translations

Already Well Covered
====================
* Time of day, date and timezones by [DateTime](http://msdn.microsoft.com/en-us/library/system.datetime.aspx) or [NodaTime](https://www.nuget.org/packages/NodaTime)
