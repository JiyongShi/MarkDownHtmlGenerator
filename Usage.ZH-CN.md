# MarkDownHtmlGenerator #

## 描述 ##

		此项目从指定文件夹，通过指定文件名或通配符方式转换markdown文件为html文件

## 参数 ##

* -p/-path 

	包含Markdown文件的目录。参数必须提供！示例: -p C:\Projects\ProjA\Apis

* -o/--output

	保存生成的html文件的目录。示例 : -o C:\Projects\ProjA\Apis\Html
	
* -f/--files

	Markdown 文件列表，逗号分隔。 示例: -f Index.md,Base.md,UserValidation.md

* -s/--searchpattern

	Markdown文件搜索通配符。 示例: -s *.md or -s Index.md

* -v/--verbose
	
	输出详细消息。示例: -v
	
* --help
	
	显示帮助。
