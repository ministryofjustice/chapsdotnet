const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyPlugin = require("filemanager-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = {
    entry: {
        main: path.resolve(__dirname, 'index.js'),
    },
    output: {
        path: path.resolve(__dirname, '../wwwroot/dist'),
        filename: "[name].bundle.js",
        publicPath: '/dist/',
    },
    watchOptions: {
        ignored: /assets/,
    },
    resolve: {
        modules: [path.resolve(__dirname, '/node_modules/')],
        alias: {
            'node_modules': path.resolve(__dirname, 'node_modules'),
            '@govuk-design-system': path.resolve(__dirname, 'node_modules/govuk-frontend/dist/govuk'),
            '@moj-design-system': path.resolve(__dirname, 'node_modules/@ministryofjustice/frontend/moj'),
            '@style-overrides': path.resolve(__dirname, '_overrides.scss')
        }
    },
    module: {
        rules: [
           {
               test: /\.(js|jsx)$/,
               exclude: /node_modules/,
               use: {
                   loader: "swc-loader"
               }
            },
            {
                test: /\.s?css$/i,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader', options: {
                            url: true,
                            sourceMap: true
                        }
                    },
                    'postcss-loader',
                    {
                        loader: 'sass-loader',
                        options: {
                            implementation: require("sass"),
                            sassOptions: {
                                quietDeps: true,
                                silenceDeprecations: ['mixed-decls', 'color-functions', 'global-builtin', 'import'],
                            }
                        }
                    }
                ]
            },
            {
                test: /\.(jpg|png|svg|gif)$/,
                type: 'asset/resource'
            },
        ]
    },

    plugins: [
        new CopyPlugin({
            runTasksInSeries: false,
            events: {
                onStart: {
                    mkdir: [
                        path.resolve(__dirname, "assets"),
                        path.resolve(__dirname, "assets/fonts"),
                        path.resolve(__dirname, "assets/images"),
                    ],
                    copy: [
                        { source: path.resolve(__dirname, 'node_modules/govuk-frontend/dist/govuk/assets/fonts'), destination: path.resolve(__dirname, "assets/fonts"), options: { overwrite: false } },
                        { source: path.resolve(__dirname, 'node_modules/govuk-frontend/dist/govuk/assets/images'), destination: path.resolve(__dirname, "assets/images"), options: { overwrite: false } },
                        { source: path.resolve(__dirname, 'node_modules/@ministryofjustice/frontend/moj/assets/images'), destination: path.resolve(__dirname, "assets/images"), options: { overwrite: false } },
                    ]
                }
            }
        }),
        new MiniCssExtractPlugin({
            filename: "[name].bundle.css",
            ignoreOrder: false
        }),
        new CleanWebpackPlugin(),
    ]
};
