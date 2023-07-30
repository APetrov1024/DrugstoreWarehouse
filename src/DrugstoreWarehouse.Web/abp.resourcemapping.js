module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs",
    ],
    mappings: {
        "@node_modules/tabulator-tables/dist/js/tabulator.min.js*": "@libs/tabulator-tables/js/",
        "@node_modules/tabulator-tables/dist/css/tabulator.min.css*": "@libs/tabulator-tables/css/",
    }
};
