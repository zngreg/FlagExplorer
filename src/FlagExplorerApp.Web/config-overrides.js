module.exports = {
    jest: (config) => {
      // Add custom Jest config here
      return config;
  },
  setupFilesAfterEnv: ["./jest.setup.ts"],
  };  