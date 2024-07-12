# ScriptBuilder
The Script Builder is a freeware tool for creating text files containing similar content contained in blocks with different values​​. For example, in configuration scripts to enter mass data, etc.

For this purpose, the variable data is loaded by importing a table in the tool. The data fields can then be used as variables in a Text Block template, from which the configuration script should be generated. Script Builder then repeats for each record - meaning each row of the table - the corresponding text blocks and thus generates the whole script.

The data input and the specification of the data blocks is very flexible. Data can also be output in various output files.

The Script Builder was created as a temporary solution, when such a function was required but there was no tool available. Starting as an Excel macro quickly became apparent that the execution was too slow when large amounts of data need to be processed. So development was switched to the programming language C# and the .Net Framework. The tool in its current form is the result of several hours and weeks of work. I completely developed it on my own.

To make it accessible to other people who are facing similar challenges, this website has now emerged. I hope other users can quickly identify a benefit and use it to their own work easier.

### PREREQUISITES

The App needs .Net-Framework 4.8 to run. No further libraries needed. It can be run without installation. There is no interaction with the windows registry. Deletion of the app folder is enough for deinstalation without any traces left.

### CONTRIBUTE TO THE PROJECT

I'm developing this project with Visual Studio Community 2022. Feel free to help me. You can add a translation in other Languages or help update the code.
This project was created a long time ago. Since that time it was not ment to be developed in a community. My C# skills improved since then but I did not find the time to work over this project, so I decided to load it into GitHub.

THe code is not well commented. If you want to contribute let me know. If you have problems to understand my code just ask and I try to remember and help you.

### CURRENT VERSION

Version: 0.9 BETA

### COPYRIGHT INFORMATION

Copyright (C) of Script Builder programm: Max Zoeller, 2009 - 2011
Copyright (C) of Icons used in the programm: http://led24.de/ (see icons-readme.txt and icons-license.txt)

### LICENSE INFORMATION

The application is free for personal and commercial use.

http://creativecommons.org/licenses/by-sa/3.0/

You are free:
* to Share - to copy, distribute and transmit the work

Under the following conditions:
Attribution — You must attribute the work in the manner specified by the author or licensor (but not in any way that suggests that they endorse you or your use of the work).

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Max Zoeller
10/09/2009

### VERSION HISTORY

0.9.0.0 - First Beta Build with basic functions (Import via CSV and quickimport for Excel and Deployment Service clipboard data; usage of simple variables; Header, Body, Footer support; Export in different encodings).
0.9.0.3 - Implementation of support of substring variables.
0.9.0.4 - Fix to enable the usage of more then one substring variable in the same line in body textbox.
0.9.0.5 - Fix in CSV importing (empty lines and lines with a hash # at the beginning are ignored; different length of lines are supported).
        - Fix in usage of Footer (Ticket# 00001).
        - Information message, when .Net-Framework is not in correct version (v3.5 is needed - version number 2.0.50727.3053) (Change Request #00001).
0.9.1.0 - Implementation of a function, to use first row of data input table as headers for the columns (variables).
        - Implementation of a function, to Import and Export the Set of the pattern text boxes and the table.
        - Integration of these function in the makro-keys function.
        - New Icon for function "Import Table" (old one is used for function "Import Pattern" now).
0.9.2.0 - Fix in function to use first row of data input table as headers: when new header row has less fields as the current header or fields contain spaces.
        - Considerable improvement in performance while generating output-file.
        - Implementation of a function, to verify the template text (Comments, If-Blocks and Variables were marked within the template colorfully).
        - Renaming "substring variables" to "complex variables" due to better understanding.
        - Implementation of a dialog, to check complex variables on a test-value to verify its correctness.
        - Implementation of comments (which will not be written to the output-file) and if/else-blocks for use in the template text.
        - Implementation of a dialog, to create a if/else-block via wizzard.
        - New programm option to configure the behavior of the start directory of the browse dialogs (Change Request #00002).
0.9.2.1	- Implementation of icons to the template-tabs
0.9.2.2 - Fix in usage of variables, when using more than one different variable in one row of the template (Ticket# 00002).
0.9.3.0 - Implementation of a function to use more than 3 different template blocks (Header, Body, Footer)
        - Now it is possible to choose between two types of template blocks (Once and Multi) and arrange as many of them as needed into an output file.
        - Implementation of a function to generate more than one output file at once based on the same imput table.
        - Redesign of the template area to meet the requirement of the new functions.
        - Fix in the useablility of the options configuration.
0.9.3.1 - Fix of a problem with the read/write process of the options.xml which caused a failure at application start.
0.9.3.2 - Fix of a problem in opening the options dialog.
        - Implementation of a debuggig option to be activated when asked to do so by support.
0.9.3.3 - Fix of a problem in If-blocks when comparative operator is not double value (Ticket# 00003).
0.9.3.4 - First steps to implement other languages for localization.
        - Fix of a problem with Preference Dialog Window and the setup of Makro Keys (Ticket# 00004).
        - Fix of a problem with Makro Keys not been loaded from options.xml after applocation start (Ticket# 00005). 
0.9.3.5 - Fix of a exeption when opening preferences dialog and certain options like OSV scripting dokumentation were not available (Ticket# 00006).
        - Fix of a problem when starting the tool without the correct laguage files available which were introduced in 0.9.3.4 (Ticket# 00007).
0.9.3.6 - Fix of a problem in the import function of templates (Ticket# 00008).
0.9.9.0 - Localization for German (main) and English implemented.
		- Implementation of Serial File functionalitys
0.9.9.1 RELEASE CANDIDATE 1
        - Introduction of the new XML format for template exports.
        - Implementation of a function to stip single columns of a source table.
        - Fix with the comments. URLs will not longer be treated as comments (Ticket# 00009).
0.9.9.2 RELEASE CANDIDATE 2
		- Transfer to .Net-Framework 4.8.
		- Minor fixes with creation of options.xml when not existant (include example makros).
		- Added shortcuts to delete or rename a template node.
		- Minor change in user experience, to have the default template node selected on startup.
		- Housekeeping of the project files for github.