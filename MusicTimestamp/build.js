const fs = require('fs')
const cp = require('child_process')
const yargs = require('yargs-parser')
const rimraf = require('rimraf')
const archiver = require('archiver')
const info = require('./Info.json')
const path = require('path')
const { findSteamAppById } = require('find-steam-app')

setImmediate(async () => {
    const args = yargs(process.argv)

    rimraf.sync('Release')

    cp.execSync(`dotnet "C:\\Program Files\\dotnet\\sdk\\5.0.203\\MSBuild.dll" /p:Configuration=${args.release ? 'Release' : 'Debug'}`)

    fs.mkdirSync('Release')

    fs.copyFileSync(`./bin/${args.release ? 'Release' : 'Debug'}/${info.Id}.dll`, `./Release/${info.Id}.dll`)

    if (args.release) fs.copyFileSync('./Info.json', './Release/Info.json')
    else {
        const Info = JSON.parse(fs.readFileSync('./Info.json'))
        Info.Version += "_dev";

        fs.writeFileSync('./Release/Info.json', JSON.stringify(Info, null, 4))
    }

    if (args.release) {
        const zip = archiver('zip', {})
        const stream = fs.createWriteStream(path.join(__dirname, `${info.Id}-${info.Version}.zip`))
        zip.pipe(stream)
        zip.directory('Release/', info.Id)
        await zip.finalize()

        console.log('Successful.')
    } else {
        if(!args.norestart) await cp.exec('taskkill /f /im "a da"*')

        setTimeout(async () => {
            const appPath = await findSteamAppById(977950)
            const modPath = path.join(appPath, 'Mods', info.Id)
            const r68ModPath = path.join('D:\\steam\\steamapps\\common\\A Dance of Fire and Ice_r68', 'Mods', info.Id)
            if(!args.norestart) rimraf.sync(modPath)
            if(!args.norestart) rimraf.sync(r68ModPath)
            if(!args.norestart) fs.mkdirSync(modPath)
            if(!args.norestart) fs.mkdirSync(r68ModPath)
            fs.copyFileSync(`Release/${info.Id}.dll`, path.join(modPath, info.Id + '.dll'))
            fs.copyFileSync('Release/Info.json', path.join(modPath, 'Info.json'))
            fs.copyFileSync(`Release/${info.Id}.dll`, path.join(r68ModPath, info.Id + '.dll'))
            fs.copyFileSync('Release/Info.json', path.join(r68ModPath, 'Info.json'))

            if(!args.norestart) try {
                await cp.exec('explorer steam://rungameid/977950')
            } catch (e) {
            }

            console.log('Successful.')
        }, 1200)
    }
})