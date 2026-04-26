export default async (req, context) => {
    const API_KEY = Netlify.env.get("API_KEY");
    const API_URL = Netlify.env.get("API_URL");

    // Ensure trailing slash
    const baseUrl = API_URL.endsWith("/") ? API_URL : API_URL + "/";

    // Remove the /TMDB/ prefix
    const url = new URL(req.url);
    const path = url.pathname.replace("/TMDB/", "");

    // Build target TMDB URL
    const targetUrl = `${baseUrl}${path}?${url.searchParams}`;

    // Proxy request
    const response = await fetch(targetUrl, {
        headers: {
            Authorization: `Bearer ${API_KEY}`
        },
        method: req.method
    });

    return new Response(response.body, {
        status: response.status,
        headers: response.headers
    });
};

// Netlify Edge Function routing
export const config = {
    path: "/TMDB/*"
};
