/// <binding ProjectOpened='all, watch' />
'use strict';
module.exports = function (grunt) {
    grunt.initConfig({
        clean: ["wwwroot/scripts/*", "temp/"],

        // TODO: Minify JS eventually
        //uglify: {
        //    all: {
        //        src: ["wwwroot/scripts/*.js"],
        //        dest: "wwwroot/scripts/*.min.js"
        //    }
        //},

        less: {
            development: {
                files: {
                    'wwwroot/css/Navbar.css': 'Styles/Navbar.less'
                }
            },
            production: {
                files: {
                    'wwwroot/css/Navbar.css': 'Styles/Navbar.less'
                }
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
            files: ["Scripts/*.ts", "Styles/*.less"],
            tasks: ["all"]
        },
        
        karma: {
            unit: {
                configFile: 'Scripts/karma.conf.js'
            }
        }
        //TODO: add watch for css.min, issue with watch.

    });

    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-ts');
    grunt.loadNpmTasks('grunt-karma');
    grunt.loadNpmTasks('grunt-contrib-less');

    grunt.registerTask("all", ['clean', 'ts', 'cssmin', 'less']); //,'uglify']); // TODO: Minify JS eventually
    grunt.registerTask("tsrecompile", ['clean', 'ts']);
    grunt.registerTask("test", ['karma']);

};