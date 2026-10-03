// Test-only in-memory storage and fail-closed platform adapters. No WinRT/UI emulation.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Windows.Foundation.Metadata { public sealed class WebHostHiddenAttribute : Attribute {} }
namespace Windows.UI.Xaml.Controls { internal class NamespaceMarker {} }
namespace Windows.UI.Xaml.Data { internal class NamespaceMarker {} }
namespace Windows.Storage {
 public sealed class ApplicationData { public static readonly ApplicationData Current = new ApplicationData(); public readonly ApplicationDataContainer RoamingSettings = new ApplicationDataContainer(); }
 public sealed class ApplicationDataContainer { public readonly Dictionary<string,object> Values = new Dictionary<string,object>(); }
}
namespace Windows.Storage.Streams {
 public sealed class InMemoryRandomAccessStream { public InMemoryRandomAccessStream() { throw new InvalidOperationException("Image IO forbidden"); } public void Seek(ulong offset) { throw new InvalidOperationException(); } }
 public sealed class DataWriter { public DataWriter(InMemoryRandomAccessStream stream) { throw new InvalidOperationException(); } public void WriteBytes(byte[] data) { throw new InvalidOperationException(); } public Task<uint> StoreAsync() { throw new InvalidOperationException(); } }
}
namespace Windows.UI.Xaml.Media.Imaging { public sealed class BitmapImage { public void SetSource(Windows.Storage.Streams.InMemoryRandomAccessStream stream) { throw new InvalidOperationException(); } } }
namespace MalukahSongs { internal static class App { public static DataModel.MainViewModel ViewModel; } }
