const result = require('esbuild').buildSync({
    entryPoints: ['src/main.ts'],
    outdir: './dist',
    target: ['es2022'],
    minify: true,
    color: true,
    metafile: true,
    bundle: true
});

require('fs').writeFileSync('./dist/meta.json',
    JSON.stringify(result.metafile));