// MCMS.Web/tailwind.config.js
const defaultTheme = require('tailwindcss/defaultTheme');

module.exports = {
    content: [
        './Views/**/*.cshtml',
        './wwwroot/js/**/*.js'
    ],
    safelist: [
        'hidden',
        'ki-filled',
        'ki-outline',
        'ki-duotone',
        'ki-solid',
        { pattern: /^apexcharts-.*$/ }
    ],
    darkMode: 'class',
    theme: {
        extend: {
            // Using the same extend configuration from your shared tailwind config
            fontFamily: {
                sans: ['Inter', 'system-ui', 'sans-serif'],
            },
            colors: {
                primary: {
                    DEFAULT: 'var(--tw-primary)',
                    active: 'var(--tw-primary-active)',
                    light: 'var(--tw-primary-light)',
                    clarity: 'var(--tw-primary-clarity)',
                    inverse: 'var(--tw-primary-inverse)',
                },
                success: {
                    DEFAULT: 'var(--tw-success)',
                    active: 'var(--tw-success-active)',
                    light: 'var(--tw-success-light)',
                    clarity: 'var(--tw-success-clarity)',
                    inverse: 'var(--tw-success-inverse)',
                },
                // Add the rest of the color configuration as in the provided tailwind.config.js
            }
            // Include other extensions from your shared config
        },
        custom: ({ theme }) => ({
            // Include the custom components configuration from your shared config
            layouts: {
                demo1: {
                    sidebar: {
                        width: {
                            desktop: '280px',
                            desktopCollapse: '80px',
                            mobile: '280px'
                        }
                    },
                    header: {
                        height: {
                            desktop: '70px',
                            mobile: '60px'
                        }
                    }
                }
            }
        })
    },
    plugins: [
        // Include references to Metronic plugins or create simplified versions
    ]
};