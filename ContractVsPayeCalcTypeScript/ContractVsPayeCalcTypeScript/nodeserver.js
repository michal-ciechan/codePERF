var Webpack = require('webpack');
var WebpackDevServer = require('webpack-dev-server');
var Config = require('./webpack.config');

new WebpackDevServer(Webpack(Config), {
  publicPath: Config.output.publicPath,
  hot: true,
  historyApiFallback: true
}).listen(3001, 'localhost', function (err, result) {
  if (err) {
    return console.log(err);
  }

  console.log('Listening at http://localhost:3001/');
});
