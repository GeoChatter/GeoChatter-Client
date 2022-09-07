const sass = require("sass");
const fs = require('fs');
const bdir = './dist/' // For tailwind stuff './build/';

sass.render({
    file: "./src/main.scss"
}, function(err, result)
{
    if (err) return console.error(err)
    console.log("Successfully compiled...\r\n", result.stats)

    let b = Buffer.from(result.css);
    console.log(b);

    if (!fs.existsSync(bdir))
    {
        fs.mkdirSync(bdir);
    }
    fs.writeFileSync(bdir + 'main.css', b);
});