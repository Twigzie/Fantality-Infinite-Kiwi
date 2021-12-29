using System;
using System.Reflection;
using System.Threading.Tasks;
using Kiwi.Classes.Commands;
using static Kiwi.Classes.CommandHelpers;
using static Kiwi.Classes.ConsoleHelpers;

namespace Kiwi {

    internal class Program {

        [STAThread]
        public static void Main(string[] args) {

            try {

                Init();

                while (true) {

                    var command = new Command(Console.ReadLine());
                    if (command.Valid == false)
                        continue;
                    else {
                        switch (command.Name.ToLower()) {
                            case "help":
                                HelpCommand();
                                continue;
                            case "clear":
                                ClearCommand();
                                continue;
                            case "about":
                                AboutCommand();
                                continue;
                            case "github":
                                GithubCommand();
                                continue;
                            case "browse":
                                BrowseCommand();
                                continue;
                            case "update":
                                UpdateCommand();
                                continue;
                            case "extract":
                                ExtractCommand();
                                continue;
                            default:
                                continue;
                        }
                    }

                }

            }
            catch (Exception ex) {
                Error(ex);
            }
            finally {
                Close();
            }

        }

        private static void Init() {

            Console.Title = $"Kiwi | Font Extractor | {Environment.UserName}";

            ClearCommand();

        }

    }

}