// netlify/edge-functions/tmdb.js

export default async function handler(req) {
  const API_KEY = Netlify.env.get("API_KEY");
  const API_URL = Netlify.env.get("API_URL"); // e.g. https://api.themoviedb.org/3

  const incoming = new URL(req.url);

  // Remove /TMDB prefix
  const tmdbPath = incoming.pathname.replace(/^\/TMDB/, "");

  // Build TMDB target URL
  const target = new URL(API_URL + tmdbPath);

  // Copy query params
  incoming.searchParams.forEach((v, k) => target.searchParams.append(k, v));

  // Add API key
  target.searchParams.set("api_key", API_KEY);

  // Proxy request
  const resp = await fetch(target.toString(), {
    method: req.method,
    headers: {
      "Content-Type": "application/json"
    }
  });

  const body = await resp.arrayBuffer();

  return new Response(body, {
    status: resp.status,
    headers: {
      "content-type": resp.headers.get("content-type") || "application/json"
    }
  });
}

export const config = {
  path: "/TMDB/*"
};

