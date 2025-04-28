# Frontend

## Quickstart

### Using an existing component

Simple components that don't take any parameters, such as the Primary Navigation can simply be added to a Razor view like this:

`<partial name="PrimaryNavigation" />`

More complicated components, such as the Breadcrumbs component, must first be initialised with the required properties, e.g.

```
    List<BreadcrumbModel> breadcrumbs = [
                new BreadcrumbModel { Label = "Home", Url = "/"},
                new BreadcrumbModel { Label = "Admin", Url = "/Admin"},
    ];
    var breadcrumbsModel = new BreadcrumbsModel(breadcrumbs);`
```

Then you can pass that to the partial:

`<partial name="Breadcrumbs" model="breadcrumbsModel" />`

### Adding a New Component

To build a new component:

1. Make a new folder called ComponentName in `Frontend/Components`.
2. Add a Razor view called `_ComponentName.cshtml` and build the component template.
3. If the component will take parameters, e.g. a label or a url, add a Model called `ComponentModel.cs` and outline the structure.
4. Add a `ComponentName.scss` file and import/forward the relevant styling from the MoJ/Gov UK design system.
5. Add a `ComponentName.js` and make sure to add `import './ComponentName.scss';`. Wrap any additional JavaScript in a default export, i.e. `export default function () {}`.
6. Finally import `ComponentName.js` in `Frontend/index.js`.

### Gotchas

##### Razor can't find the component

Ensure that the value passed to `name` **doesn't include the leading slash**.

As `Frontend/Components` is a custom subfolder, the allowed custom routes are listed in `RazorOptions()` in `Program.cs`. 

##### My Area template is using the old layout
There's a filter in `Helpers/Helpers.cs` called `LayoutFilter` that decides whether to use the old or new `_Layout.cshtml` file. You may have to add the controller name to the `UpdatedPageControllers` array.

##### Forms and fields

There wasn't an easy way to write re-usable form fields, whilst preserving the validation functionality that we already have. As such, complete forms are currently in `Views/Shared/EditorTemplates`.

Ideally we can update this in the future, possibly using something like this https://github.com/christianlevesque/RazorForms


## Architecture

### Structure
This folder contains reusable frontend components and the scripts required to bundle them to be used by the main CHAPS application.

### Components
Most frontend components can be found here. Each component is self-contained and includes:

* `ComponentModel.cs` - The associated C# model.
* `_Component.cshtml` - The Razor partial view template.
* `Component.js` - The JavaScript file.
* `Component.scss` - The stylesheet.

As we're using the MoJ<sup name="footnoteref1">[1](#footnote1)</sup> and GOV UK<sup name="footnoteref2">[2](#footnote2)</sup> design systems, most of the SCSS files will only contain the `@forward`/`@import` rule for the associated MoJ Design System component or Gov UK design system, with any overrides/tweaks written below.
### webpack.config.js
JavaScript, SCSS and static assets are bundled using `Webpack` and output to the `wwwroot/dist` directory for use by the main CHAPS application.
### assets
The Webpack script copies the static assets required from the design systems. These don't need to be added to GitHub as they'll be bundled into `wwwroot/dist` as part of the build process.
### _Layout.cshtml and _ViewImports.cshtml
These are copies of the files found in `Views/Shared` but with links to the assets in `wwwroot/dist`, and including reusable components that should appear on every page, e.g. the footer. Once the upgrade is complete we can replace the originals.
### _overrides.scss
This file stores SCSS variables that we're overriding across all of the design system components, for example the `max-width` is set here.
### _base.scss
`_base.scss` is where we're importing any base or utility classes from the design systems.

---
<span name="footnote1">[1]</span> https://design-patterns.service.justice.gov.uk/ [&crarr;](#footnoteref1)

<span name="footnote1">[2]</span> https://design-system.service.gov.uk/ [&crarr;](#footnoteref2)