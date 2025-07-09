docker-compose up -d

possible future improvements:
- proper url validation in backend. maybe automatic testing on creation if url is valid/exists
- authentication (+ user system? admin, "normal" user, etc.)
- confirmation dialog on delete/activate/deactivate in frontend
- integration tests
- frontend tests
- simple payment service uses development server (if only used for testing, this is fine)
- uniform api responses (create middleware which intercepts all responses and returns a standard format); especially for errors
- add more currencies to enum in backend
- pagination for list endpoints in backend/frontend (list all providers especially)
- serve angular app with nginx in production
