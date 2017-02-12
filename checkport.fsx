
let port = 8083
System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners()
|> Seq.exists (fun x -> x.Port = port)
