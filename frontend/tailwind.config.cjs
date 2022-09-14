/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    colors:{
      
      "white": "#FFFFFF",
      "light_background": "#efefef",
      "background": "#bf7c2a",
      "brown": "#734319",
      "interactive": "#f2ca80",
      "accent": "#a6401b",
      "accept2": "#f2ab27"
    },
    extend: {},
  },
  plugins: [
    require('@tailwindcss/aspect-ratio'),
    require('@tailwindcss/forms')
  ],
}
