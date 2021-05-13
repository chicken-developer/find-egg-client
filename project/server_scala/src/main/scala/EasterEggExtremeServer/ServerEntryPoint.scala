package EasterEggExtremeServer

import EasterEggExtremeServer.Service.GameServer
import akka.actor.ActorSystem
import akka.http.scaladsl.{ConnectionContext, Http}
import akka.stream.Materializer

import java.io.InputStream
import java.security.{KeyStore, SecureRandom}
import javax.net.ssl.{KeyManagerFactory, SSLContext, TrustManagerFactory}
import scala.io.StdIn

object ServerEntryPoint {
  def main(args: Array[String]): Unit = {

    implicit val system = ActorSystem()
    implicit val materializer = Materializer
    implicit val executionContext = system.dispatcher

    val key: KeyStore = KeyStore.getInstance("PKCS12")
    val keyStoreFile: InputStream = getClass.getClassLoader.getResourceAsStream("myKeystore.p12")
    val password = "MY_PASSWORD".toCharArray
    key.load(keyStoreFile, password)

    val keyManagerFactory = KeyManagerFactory.getInstance("SunX509")
    keyManagerFactory.init(key, password)

    val trustManagerFactory = TrustManagerFactory.getInstance("SunX509")
    trustManagerFactory.init(key)

    val sslContext: SSLContext = SSLContext.getInstance("TLS")
    sslContext.init(keyManagerFactory.getKeyManagers, trustManagerFactory.getTrustManagers, new SecureRandom)

    val httpsConnectionContext = ConnectionContext.httpsServer(sslContext)


    val gameService = new GameServer()

    val localHost = "127.0.0.1"
    val localPort = 8005

    val herokuHost = "0.0.0.0"
    val herokuPort: Int = sys.env.getOrElse("PORT", "8005").toInt

    val bindingFutureWithSecurity = Http().newServerAt(localHost, localPort).enableHttps(httpsConnectionContext).bindFlow(gameService.GameFinalRoute) // https://

    println(s"Server is progressing...\nPress RETURN to stop...")
    StdIn.readLine()
    bindingFutureWithSecurity
      .flatMap(_.unbind())
      .onComplete(_ => system.terminate())
  }
}
