/// <binding AfterBuild='all' />
'use strict';
var childProcess = require("child_process");

module.exports = function (grunt) {
    grunt.initConfig({
        clean: ["wwwroot/scripts/*", "temp/"],

        uglify: {
            all: {
                src: ["wwwroot/scripts/*.js"],
                dest: "wwwroot/scripts/*.min.js"
            }
        },

        cssmin: {
            options: {
                shorthandCompacting: false,
                roundingPrecision: -1
            },
            target: {
                files: {
                    "wwwroot/css/site.min.css": ["wwwroot/css/site.css"]
                }
            }
        },

        ts: {
            default: {
                tsconfig: {
                    tsconfig: 'Scripts/tsconfig.json',
                },
            },
        },

        watch: {
            files: ["Scripts/*"],
            tasks: ["tsrecompile"]
        }

    });

    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-ts');

    grunt.registerTask("all", ['clean', 'ts', 'cssmin', 'uglify']);
    grunt.registerTask("tsrecompile", ['clean', 'ts']);

};
