import http from 'http';
import { config } from "./config";

http.createServer(function (req: any, res: { write: (arg0: string) => void; end: () => void; }) {
  res.write(`I'm alive.  CLIENT_ID: ${config.CLIENT_ID}`);
  res.end();
}).listen(8080);
