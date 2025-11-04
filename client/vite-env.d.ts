/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_REACT_APP_SERVER_URL: string;
}
interface ImportMeta {
  readonly env: ImportMetaEnv;
}
