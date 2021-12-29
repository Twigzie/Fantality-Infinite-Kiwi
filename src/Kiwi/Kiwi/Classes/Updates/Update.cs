using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Kiwi.Classes.Updates {

    [DataContract]
    internal class UpdateVersion {

        #region Ignored

        [IgnoreDataMember]
        public Version Current {
            get => Assembly.GetExecutingAssembly().GetName().Version;
        }
        [IgnoreDataMember]
        public bool IsAvailable {
            get {
                if (Current == null || string.IsNullOrEmpty(Available))
                    return false;
                return new Version(Available.Replace("v", "")) > Current;
            }
        }

        #endregion

        public UpdateVersion() {

        }

        [DataMember(Name = "html_url", IsRequired = true, Order = 0)]
        public string Url { get; set; }
        [DataMember(Name = "body", IsRequired = true, Order = 1)]
        public string Details { get; set; }
        [DataMember(Name = "name", IsRequired = true, Order = 2)]
        public string Available { get; set; }

    }

    internal static class Update {

        public static async Task<UpdateVersion> GetUpdates() {
            try {
                using (var client = new WebClient()) {
                    client.Headers.Add(HttpRequestHeader.UserAgent, $"KIWI/{Assembly.GetExecutingAssembly().GetName().Version.ToString()} (release; PC)");
                    using (var stream = new MemoryStream(await client.DownloadDataTaskAsync(new Uri("https://api.github.com/repos/Twigzie/Fantality-Infinite-Kiwi/releases/latest")))) {
                        var data = new DataContractJsonSerializer(typeof(UpdateVersion));
                        return (UpdateVersion)data.ReadObject(stream);
                    }
                }
            }
            catch {
                return new UpdateVersion();
            }
        }

    }

}