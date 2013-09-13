# MarkDownHtmlGenerator #

This Program is used to generate html from markdown files which are in special folder.

## Usage ##

[English Usage](Usage.md)

[Chinese Usage](Usage.ZH-CN.md)

## Installation ##

### Use Source Code ###

Use Visual Studio 2012 or MSBuild compile MarkDownHtmlGenerator.csproj, then use executable file which is generated in dist folder.

### Use Nuget ###

Search MarkDownHtmlGenerator, install it.

## Integeration ##

### Project ###
You can integrate this tool into your project build progress by invoke in after-build event.

	$(SolutionDir)\packages\MarkDownHtmlGenerator.1.0.0\tools\net40\MarkDownHtmlGenerator.exe -v -p $(TargetDir)\Api -s *.md
### CI ###
You can integrate this tool into your CI server(jenkins ...) by add an new windows command batch build step.
	.\packages\MarkDownHtmlGenerator.1.0.0\tools\net40\MarkDownHtmlGenerator.exe -v -p .\Best.Web.HSServer\Api -s *.md

## Requirements ##

.Net Framework 4.0

## Contributing ##

Shi JIyong(SHIJIYONG@HOTMAIL.COM) © [Best Logistics Technology Co,.Ltd](http://www.800best.com)

## Credits ##

Commandline by Giacomo Stelluti Scala 

MarkdownDeep by Topten software

## License ##
[LicenseDetail](https://github.com/JiyongShi/MarkDownHtmlGenerator/blob/master/LICENSE)

## Donate ##
[DonateDetail](https://github.com/JiyongShi/MarkDownHtmlGenerator/blob/master/Donations.md)