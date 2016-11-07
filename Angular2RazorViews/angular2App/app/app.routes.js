"use strict";
var router_1 = require("@angular/router");
var project_component_1 = require("./project/project.component");
var appRoutes = [
    { path: "", component: project_component_1.ProjectComponent },
    {
        path: "about",
        loadChildren: "about.bundle.js"
    }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routes.js.map