// netlify/edge-functions/tmdb.js

export default async function handler(req) {
  const API_KEY = Netlify.env.get("API_KEY");
  const API_URL = Netlify.env.get("API_URL"); // e.g. https://api.themoviedb.org/3

  if (!API_KEY || !API_URL) {
    return new Response("Edge function misconfigured: missing API_KEY or API_URL", { status: 500 });
  }

  const incoming = new URL(req.url);

  // Remove /tmdb prefix
  const tmdbPath = incoming.pathname.replace(/^\/tmdb/, "");

  // Build TMDB target URL, stripping any trailing slash from API_URL to avoid double-slash
  const target = new URL(API_URL.replace(/\/$/, "") + tmdbPath);

  // Copy query params
  incoming.searchParams.forEach((v, k) => target.searchParams.append(k, v));

  // Proxy request using Bearer token authentication (Read Access Token)
  const resp = await fetch(target.toString(), {
    method: req.method,
    headers: {
      "Authorization": `Bearer ${API_KEY}`,
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
  path: "/tmdb/*"
};

