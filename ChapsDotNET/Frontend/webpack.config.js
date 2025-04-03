const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyPlugin = require("copy-webpack-plugin");
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
    resolve: {
        modules: [path.resolve(__dirname, '/node_modules/')],
        alias: {
            'node_modules': path.resolve(__dirname, 'node_modules'),
            '@govuk-design-system': path.resolve(__dirname, 'node_modules/govuk-frontend/dist/govuk'),
            '@moj-design-system': path.resolve(__dirname, 'node_modules/@ministryofjustice/frontend/moj'),
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
                use: [MiniCssExtractPlugin.loader, { loader: 'css-loader', options: { url: true, sourceMap: true } }, 'postcss-loader', 'sass-loader']
            },
            {
                test: /\.(jpg|png|svg|gif)$/,
                type: 'asset/resource'
            },
        ]
    },

    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].bundle.css",
            ignoreOrder: false
        }),
        new CopyPlugin({
            patterns: [
                { from: path.resolve(__dirname, 'node_modules/govuk-frontend/dist/govuk/assets'), to: path.resolve(__dirname, "assets") },
            ],
        }),
        new CleanWebpackPlugin(),
    ]
};