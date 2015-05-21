
## General ##

- Add a `sync` command that will sync the output to Google Cloud Storage, etc.
- Add a `serve` command that spins up a local web server for the output directory, and watches the input directories for changes
- Add a link checker, either as a standalone command or an option during site generation

## Generator ##

- Need to copy images, styles, etc.
- Process .gitignore files to avoid dirs we don't want to see
- Should (optionally) minify JS
- Allow custom (site-specific) Razor templates
- Figure out how to use partial views in RazorEngine
- The relative path to the script directory should be part of DocumentModel
- The relative path to the "main" image directory should be part of DocumentModel
- Should bundle javascript files (and perhaps css)


## Other Markup ##

- [AsciiDoc](http://www.methods.co.nz/asciidoc/)?
- Look at the [lightweight markup language](https://en.wikipedia.org/wiki/Lightweight_markup_language) page on Wikipedia for ideas
