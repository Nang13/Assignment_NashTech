/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      borderColor: {
        'custom-rgba': 'rgba(0, 0, 0, 0.15)',
      },
    },
  },
  plugins: [],
}

