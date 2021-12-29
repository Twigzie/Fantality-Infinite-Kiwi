using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kiwi.Classes.Updates;

namespace Kiwi.Classes {

    internal class Font { }

    internal static class FileHelpers {

        public static List<string> GetFontsFromPath(string path) {
            try {
                return Directory.GetFiles(path).Where(n => Path.GetExtension(n) == ".wpf_font").ToList();
            }
            catch (Exception) {
                throw;
            }
        }
        public static List<string> GetResourcesFromFontName(string path, string name) {
            try {
                return Directory.GetFiles(path).Where(n => n.Contains(Path.GetFileName(name)) && n.Contains("font_file_resource")).ToList();
            }
            catch (Exception) {
                throw;
            }
        }

    }
    internal static class CommandHelpers {

        public static void HelpCommand() {
            ConsoleHelpers.Title();
            Console.WriteLine($"\t> Help");
            Console.WriteLine();
            Console.WriteLine($"\t\t> Command: help");
            Console.WriteLine($"\t\t> Description: Shows this message");
            Console.WriteLine();
            Console.WriteLine($"\t> Clear");
            Console.WriteLine();
            Console.WriteLine($"\t\t> Command: clear");
            Console.WriteLine($"\t\t> Description: Clears this console.");
            Console.WriteLine();
            Console.WriteLine($"\t> About:");
            Console.WriteLine();
            Console.WriteLine($"\t\t> Command: about");
            Console.WriteLine($"\t\t> Description: Shows application credits");
            Console.WriteLine();
            Console.WriteLine($"\t> Github:");
            Console.WriteLine();
            Console.WriteLine($"\t\t> Command: github");
            Console.WriteLine($"\t\t> Description: Opens a link to the application repo on https://github.com/");
            Console.WriteLine();
            Console.WriteLine($"\t> Updates");
            Console.WriteLine();
            Console.WriteLine($"\t\t> Command: update");
            Console.WriteLine($"\t\t> Description: Checks for updates via 'https://github.com/Twigzie/Fantality-Halo-Mohawk/releases'");
            Console.WriteLine();
            Console.WriteLine($"\t> Browse:");
            Console.WriteLine();
            Console.WriteLine($"\t\t> Command: browse");
            Console.WriteLine($"\t\t> Description: If an existing export folder if found, opens it. Otherwise, opens the applications directory.");
            Console.WriteLine();
            Console.WriteLine($"\t> Extract:");
            Console.WriteLine();
            Console.WriteLine($"\t\t> Command: extract");
            Console.WriteLine($"\t\t> Description: Opens a folder dialog and extracts fonts from the specified directory.");
            Console.WriteLine();
        }
        public static void AboutCommand() {
            ConsoleHelpers.Title();
            Console.WriteLine($"\t> Made with love by: Twigzie IRL");
            Console.WriteLine($"\t> Twitter: https://twitter.com/TwigzieIRL");
            Console.WriteLine($"\t> Github: https://github.com/Twigzie/Fantality-Infinite-Kiwi");
            Console.WriteLine($"\t> Version: {Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine($"\t> Disclaimer:");
            Console.WriteLine($"\t\t> Kiwi is provided 'as is' and I will not be responsible for any corrupt or deleted game assets.");
            Console.WriteLine($"\t\t> It works with wpf_font files, not module files.");
            Console.WriteLine($"\t\t> For information about extracting module files, please visit https://github.com/MontagueM/HaloInfiniteModuleUnpacker");
            Console.WriteLine();
        }
        public static void ClearCommand() {
            ConsoleHelpers.Title();
            Console.WriteLine($"Version: {Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine($"Directory: {Directory.GetParent(Assembly.GetExecutingAssembly().Location).Name}");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------------------------");
            Console.WriteLine();
            ConsoleHelpers.Message("Welcome!");
            ConsoleHelpers.Message("Enter a command or type 'help' for a list of available ones.");
            Console.WriteLine();
        }
        public static void GithubCommand() {
            Console.WriteLine();
            ConsoleHelpers.Info($"Opening 'https://github.com/Twigzie/Fantality-Halo-Mohawk'...");
            Console.WriteLine();
            Process.Start("https://github.com/Twigzie/Fantality-Halo-Mohawk");
        }
        public static void BrowseCommand() {
            try {

                Console.WriteLine();

                var root = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
                var path = Path.Combine(root.FullName, "export");
                if (Directory.Exists(path)) {
                    ConsoleHelpers.Info($"Existing directory was found, browsing.");
                    Console.WriteLine($"\t> '{path}'");
                    Process.Start(path);
                }
                else {
                    ConsoleHelpers.Info($"No export directory was found, browsing application directory.");
                    Console.WriteLine($"\t> '{root.FullName}'");
                    Process.Start(root.FullName);
                }

                Console.WriteLine();

            }
            catch (Exception ex) {
                ConsoleHelpers.Error(ex);
            }
        }

        //Async
        public static async void UpdateCommand() {
            await UpdateCommandAsync();
        }
        private static async Task UpdateCommandAsync() {
            try {

                ConsoleHelpers.Title();
                ConsoleHelpers.Info($"Checking for updates...");
                Console.WriteLine();

                var update = await Update.GetUpdates();
                if (update.IsAvailable) {

                    Console.WriteLine($"\t> Available: {update.Available}");
                    Console.WriteLine($"\t> Current: {update.Current}");
                    Console.WriteLine();
                    Console.WriteLine($"\t\t> An update is available.");
                    Console.WriteLine();
                    Console.WriteLine($"\t\t> Details: {update.Details}");
                    Console.WriteLine($"\t\t> Download: {update.Url}");
                    Console.WriteLine();
                    ConsoleHelpers.Info($"Checking for updates...Done!");
                    Console.WriteLine();

                }
                else {

                    Console.WriteLine($"\t> Available: {update.Available}");
                    Console.WriteLine($"\t> Current: {update.Current}");
                    Console.WriteLine();
                    Console.WriteLine($"\t> No updates available.");
                    Console.WriteLine();
                    ConsoleHelpers.Info($"Checking for updates...Done!");
                    Console.WriteLine();

                }

            }
            catch {
                Console.WriteLine($"\t> An error occurred while checking for updates.");
                Console.WriteLine();
                ConsoleHelpers.Info($"Checking for updates...Failed!");
                Console.WriteLine();
            }
        }
        public static async void ExtractCommand() {
            await ExtractCommandAsync();
        }
        private static async Task ExtractCommandAsync() {
            try {

                ConsoleHelpers.Title();
                ConsoleHelpers.Info($"Extracting resources...");
                Console.WriteLine();

                using (var FBD = new FolderBrowserDialog()) {
                    FBD.RootFolder = Environment.SpecialFolder.Desktop;
                    FBD.Description = "Select the folder which contains the font files (.wpf_font) that were extracted using 'HaloInfiniteModuleUnpacker'. See 'help' for details.";
                    FBD.ShowNewFolderButton = false;
                    FBD.SelectedPath = @"E:\Games\Halo Infinite\Dump\ui\wpf\fonttags";
                    if (FBD.ShowDialog() != DialogResult.OK) {
                        Console.WriteLine();
                        ConsoleHelpers.Info($"Extracting resources...Canceled!");
                        Console.WriteLine();
                    }
                    else {

                        var fonts = FileHelpers.GetFontsFromPath(FBD.SelectedPath);
                        if (fonts?.Count <= 0) {
                            Console.WriteLine();
                            ConsoleHelpers.Info($"Extracting resources...Skipped! No available resources were found in the target directory.");
                            Console.WriteLine();
                        }
                        else {

                            Console.WriteLine($"\t> Found '{fonts.Count}' possible fonts, extracting.");

                            foreach (var font in fonts) {

                                var fontName = Path.GetFileName(font);

                                Console.WriteLine();
                                Console.WriteLine($"\t> Processing '{fontName}'");

                                var resources = FileHelpers.GetResourcesFromFontName(FBD.SelectedPath, Path.GetFileName(font));
                                if (resources?.Count >= 1) {

                                    //wpf_font
                                    using (var fontStream = new MemoryStream(File.ReadAllBytes(font)))
                                    using (var fontReader = new FontReader(fontStream)) {

                                        fontReader.Move(212);
                                        var name = fontReader.ReadString();
                                        if (string.IsNullOrEmpty(name)) {
                                            Console.WriteLine($"\t> Skipped! An invalid font name was returned.");
                                            continue;
                                        }

                                        foreach (var resource in resources) {

                                            var resourceName = Path.GetFileName(resource);

                                            Console.WriteLine($"\t\t> Extracting '{resourceName}'");

                                            //font_file_resource
                                            using (var resourceStream = new MemoryStream(File.ReadAllBytes(resource)))
                                            using (var resourceReader = new FontReader(resourceStream)) {

                                                resourceReader.Move(96);
                                                var fontSize = resourceReader.ReadInt32();
                                                if (fontSize <= 0) {
                                                    Console.WriteLine($"\t\t> Skipped! An invalid font size was returned.");
                                                    Console.WriteLine();
                                                    continue;
                                                }

                                                resourceReader.Move(204);
                                                if (resourceReader.Position + fontSize > resourceStream.Length) {
                                                    Console.WriteLine($"\t\t> Skipped! An invalid font length was returned.");
                                                    Console.WriteLine();
                                                    continue;
                                                }

                                                try {

                                                    var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                                                    var path = Path.Combine(root, "export");
                                                    if (Directory.Exists(path) == false)
                                                        Directory.CreateDirectory(path);
                                                    var file = Path.Combine(path, $"{name}.ttf");

                                                    Console.WriteLine($"\t\t> Creating '{Path.GetFileName(file)}'");

                                                    using (var writer = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {

                                                        await writer.WriteAsync(resourceReader.ReadBytes(fontSize), 0, fontSize);

                                                        Console.WriteLine($"\t\t> Done! '/export/{name}.ttf");

                                                    }

                                                }
                                                catch (Exception ex) {
                                                    Console.WriteLine($"\t\t> Skipped! {ex.Message}");
                                                    continue;
                                                }

                                            }

                                        }

                                    }

                                }
                                else {

                                    Console.WriteLine($"\t> Processing '{font}'...Failed! Unable to locate any resources for the specified file.");
                                    Console.WriteLine();

                                }

                            }

                            Console.WriteLine();
                            ConsoleHelpers.Info($"Extracting resources...Done!");
                            Console.WriteLine();

                        }

                    }
                }

            }
            catch (Exception ex) {
                ConsoleHelpers.Error(ex);
                ConsoleHelpers.Info($"Extracting resources...Failed!");
                Console.WriteLine();
            }
        }

    }
    internal static class ConsoleHelpers {

        public static void Title() {
            Console.Clear();
            Console.WriteLine(@"--------------------------------------------------------------------------------------------------");
            Console.WriteLine(@"                           ___                        ___                                         ");
            Console.WriteLine(@"                          /  /\           ___        /  /\           ___                          ");
            Console.WriteLine(@"                         /  /:/          /__/\      /  /:/_         /__/\                         ");
            Console.WriteLine(@"                        /  /:/           \__\:\    /  /:/ /\        \__\:\                        ");
            Console.WriteLine(@"                       /  /::\____       /  /::\  /  /:/ /:/_       /  /::\                       ");
            Console.WriteLine(@"                      /__/:/\:::::\   __/  /:/\/ /__/:/ /:/ /\   __/  /:/\/                       ");
            Console.WriteLine(@"                      \__\/~|:|~~~~  /__/\/:/~~  \  \:\/:/ /:/  /__/\/:/~~                        ");
            Console.WriteLine(@"                         |  |:|      \  \::/      \  \::/ /:/   \  \::/                           ");
            Console.WriteLine(@"                         |  |:|       \  \:\       \  \:\/:/     \  \:\                           ");
            Console.WriteLine(@"                         |__|:|        \__\/        \  \::/       \__\/                           ");
            Console.WriteLine(@"                          \__\|                      \__\/                                        ");
            Console.WriteLine(@"                                                                                                  ");
            Console.WriteLine(@"                             -- A font extractor for Halo Infinite --                             ");
            Console.WriteLine(@"                                                                                                  ");
            Console.WriteLine(@"--------------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }
        public static void Close() {
            Message("Termination imminent! Exiting...");
            Console.ReadKey();
        }

        public static void Message(string message) {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Kiwi: {message}");
        }
        public static void Info(string message) {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Info: {message}");
        }
        public static void Warning(string message) {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Warning: {message}");
        }
        public static void Error(Exception source) {
            Title();
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Exception: {source.Message}");
            Console.WriteLine();
        }

    }

}