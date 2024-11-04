git pull
docker build -f src/HLS.Topup.Web.Host/Dockerfile -t hls2020/nt:api .
docker push hls2020/nt:api

pause