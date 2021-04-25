module.exports = {
  runtimeCompiler: true,
  devServer: {
    host: 'localhost'
	},
	css: {
		sourceMap: true
  },
  configureWebpack: { 
    output: {
      filename: '[name].[hash].bundle.js' 
    }
  }
};
