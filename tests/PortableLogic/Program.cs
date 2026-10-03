using System;
using MalukahSongs.DataModel;
using Windows.Storage;
class Program {
 static int checks;
 static void Check(bool ok, string name) { if (!ok) throw new Exception(name); checks++; }
 static MainViewModel Cached(object value) { var settings=ApplicationData.Current.RoamingSettings.Values; settings.Clear(); settings["tracks"]=value; return new MainViewModel(); }
 static void Main() {
  var model=Cached("{\"current_page\":2,\"total_pages\":3,\"photos\":[{\"id\":\"fixture-1\",\"name\":\"Synthetic fixture\",\"image_url\":\"https://example.invalid/fixture.jpg\"}]}");
  Check(model.Items != null && model.Items.Count==1,"cached API envelope must restore photos");
  Check(model.Items[0].Name=="Synthetic fixture" && model.Items[0].Id=="fixture-1","cache fields");
  Check(model.maxpage==3 && model.page==2,"cache page and bound");
  Check(!model.IsDataLoaded,"metadata cache does not claim images loaded");
  Check(Cached(null).Items != null,"null cache leaves non-null Items");
  Check(Cached("null").Items != null,"JSON null cache leaves non-null Items");
  Check(Cached("not json").Items.Count==0,"malformed cache ignored");
  Check(Cached("{}").Items.Count==0,"missing photos ignored");
  Check(Cached("{\"photos\":null}").Items.Count==0,"null photos ignored");
  Check(Cached("{\"photos\":[]}").Items.Count==0,"empty photos allowed");
  Check(Cached("{\"photos\":[],\"total_pages\":\"bad\"}").maxpage==1,"invalid page count ignored");
  Check(Cached("{\"photos\":[],\"total_pages\":0}").maxpage==1,"nonpositive page count ignored");
  ApplicationData.Current.RoamingSettings.Values.Clear();
  Check(new MainViewModel().Items.Count==0,"missing cache ignored");
  Console.WriteLine("PASS: "+checks+" cache checks; Newtonsoft host assembly "+typeof(Newtonsoft.Json.JsonConvert).Assembly.GetName().Version);
 }
}
