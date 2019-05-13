LCPrinter Readme

To run the example please procede as follows:

1 - Start the example scene present in this asset (LCPrinterExampleScene)
2 - LCExampleScript should be atached to Main Camera
3 - Change the printer name for your printer. Mine is "EPSON001F78 (WF-3520 Series)".
	To find your printer name please go to "Devices and Printers" on "Windows Control Panel" and copy the complete name that it appears on your printer icon.
	Note: If you do not copy the exact name, it will not work.
4 - Run the scene and click on the button. If everything is good, it will print the smile.

This plugin is simple to run and adapt. It prints a texture just by one line of code.
LCPrinter.Print.PrintTexture(texture2D.EncodeToPNG(), copies, printerName);
