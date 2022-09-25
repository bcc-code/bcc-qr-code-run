/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    colors:{
      
      "white": "#fffceb",
      "light_background": "#fffceb",
      "background": "#d19a48",
      "brown": "#423e36",
      "interactive": "#f2ca80",
      "interactive_active": "#e5be79",
      "accent": "#cb7055",
      "accept2": "#938b43"
    },
    extend: {},
  },
  plugins: [
    require('@tailwindcss/aspect-ratio'),
    require('@tailwindcss/forms')
  ],
}
