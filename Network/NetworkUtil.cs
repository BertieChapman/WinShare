using System.Net;

namespace WSNetwork {
    public class NetworkUtil {
        
        public static byte[] IpAsBytes(IPAddress ip) {

            return ip.GetAddressBytes();
        }

        public static byte[] IpAsBytes(string ip) {
            return IpAsBytes(IPAddress.Parse(ip));
        }

        /// <summary>
        /// 
        /// Apply the given IP address with the network mask provided
        /// 
        /// </summary>
        /// <param name="pIP">The IP address to apply the mask to</param>
        /// <param name="pMask">The network mask to apply</param>
        /// <returns>The IP address with the mask provided</returns>
        public static IPAddress MaskIP(IPAddress pIP, IPAddress pMask) {
            var ip = IpAsBytes(pIP);
            var mask = IpAsBytes(pMask);
            
            for(int i = 0; i < ip.Length; i++) {
                ip[i] = (byte)(ip[i] & mask[i]);
            }

            return new IPAddress(ip);
        }

        /// <summary>
        /// <see cref="MaskIP(IPAddress, IPAddress)"/>
        /// </summary>
        /// <param name="pIP"></param>
        /// <param name="pMask"></param>
        /// <returns></returns>
        public static IPAddress MaskIP(string pIP, string pMask) {
            return MaskIP(IPAddress.Parse(pIP), IPAddress.Parse(pMask));
        }

        /// <summary>
        /// 
        /// Gets the number of IP addresses per subnet for the given network mask
        /// 
        /// </summary>
        /// <param name="pMask">The network mask</param>
        /// <param name="includeNetwork">Whether to include the network address in the count</param>
        /// <param name="includeBroadcast">Whether to include the broadcast address in the count</param>
        /// <returns>The number of IP addresses per subnet for the mask</returns>
        public static int IPCountFromMask(string pMask, bool includeNetwork = true, bool includeBroadcast = true) {
            byte[] mask = IpAsBytes(pMask);
            byte[] wildcard = new byte[mask.Length];

            for(int i = 0; i < mask.Length; i++) {
                wildcard[i] = (byte)~mask[i];
            }

            int count = wildcard[mask.Length - 1];
            
            for(int i = 2; i >= 0; i--) {
                if(wildcard[i] != 0) {
                    count *= wildcard[i];
                }
            }

            if (includeNetwork) {
                count++;
            }

            if(!includeBroadcast) {
                count--;
            }

            // Need to add one for network address
            return count;
        }
    }
}
