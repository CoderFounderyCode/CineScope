// netlify/edge-functions/tmdb.js

export default async function handler(req, context) {
  const API_KEY = Netlify.env.get("API_KEY");    // from Netlify environment vars
  const API_URL = Netlify.env.get("API_URL");    // e.g., https://api.themoviedb.org/3/

  // Ensure trailing slash
  const baseUrl = API_URL.endsWith("/") ? API_URL : API_URL + "/";

  // Remove '/TMDB/' prefix
  const url = new URL(req.url);
  const newPath = url.pathname.replace("/TMDB/", "");

  // Build full TMDB URL
  const targetUrl = `${baseUrl}${newPath}${url.search}`;

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
}

// Netlify routing config
export const config = {
  path: "/TMDB/*"
};
