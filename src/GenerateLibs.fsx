#r "nuget: SimpleExec, 8.0.0"
open SimpleExec

let girFiles =
    [|
        "GLib-2.0.gir"
        "GObject-2.0.gir"
        "Gio-2.0.gir"
        "cairo-1.0.gir"
        "Pango-1.0.gir"
        "Atk-1.0.gir"
        "Gdk-3.0.gir"
        "GdkPixbuf-2.0.gir"
        "Gtk-3.0.gir"
        "Gst-1.0.gir"
        "GstBase-1.0.gir"
        "GstVideo-1.0.gir"
        "GstAudio-1.0.gir"
        "GstPbutils-1.0.gir"
    |]
    |> String.concat " "

let mutable exitCode = 0

Command.Run(
    name = "dotnet",
    args = $"run --project ../../src/GirTool/GirTool.csproj -- generate {girFiles} --output ../../src/Libs --search-path .",
    workingDirectory = "../ext/gir-files",
    handleExitCode = fun result ->
        exitCode <- result
        true)

exitCode