import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import checker from 'vite-plugin-checker';

export default defineConfig({
  plugins: [
    react(),
    checker({
      typescript: {
        tsconfigPath: './tsconfig.json', // specify your tsconfig path
        root: './', // specify your project root
        buildMode: true,
      },
    }),
  ],
  build: {
    outDir: "build", // CRA's default build output
  },
  resolve: {
    extensions: [".js", ".jsx", ".ts", ".tsx"],
  },
  test: {
    globals: true,
    environment: "jsdom",
    setupFiles: "./src/setupTests.ts",
  },
});
